using backend_planilla.Application;
using backend_planilla.Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace backend_planilla.Infraestructure
{
    public class PlanillaRepository : IPlanillaRepository
    {
        private readonly string _connectionString;

        public PlanillaRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("piTestContext")!;
        }

        public async Task<Guid> InsertarPlanillaEmpresaAsync(string cedulaJuridica, string periodo, DateTime fecha)
        {
            var query = @"INSERT INTO PlanillaDeduccionesEmpresa  (IDPlanilla, CedulaEmpresa, Periodo, FechaDeCreacion) 
                          OUTPUT INSERTED.IDPlanilla
                          VALUES (NEWID(), @CedulaJuridica, @Periodo, @Fecha);";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CedulaJuridica", cedulaJuridica);
            cmd.Parameters.AddWithValue("@Periodo", periodo);
            cmd.Parameters.AddWithValue("@Fecha", fecha);

            await conn.OpenAsync();
            return (Guid)(await cmd.ExecuteScalarAsync())!;
        }

        public async Task InsertarPlanillaEmpleadoAsync(Guid idPlanilla, ResultadoEmpleadoModel empleado)
        {
            var query = @"INSERT INTO PlanillaMensualEmpleado (
                IDPlanilla, CedulaEmpleado, SalarioBruto,
                SEMEmpleado, IVEMEmpleado, BPPOEmpleado,
                ImpuestoRenta, TotalDeduccionesEmpleado, TotalDeduccionesPatrono)
                VALUES (
                @IDPlanilla, @CedulaEmpleado, @SalarioBruto,
                @SEMEmpleado, @IVMEmpleado, @BPPOEmpleado,
                @ImpuestoRenta, @TotalDeduccionesEmpleado, @TotalDeduccionesPatrono);";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IDPlanilla", idPlanilla);
            cmd.Parameters.AddWithValue("@CedulaEmpleado", empleado.CedulaEmpleado);
            cmd.Parameters.AddWithValue("@SalarioBruto", empleado.SalarioBruto);
            cmd.Parameters.AddWithValue("@SEMEmpleado", empleado.Deducciones.SEMEmpleado);
            cmd.Parameters.AddWithValue("@IVMEmpleado", empleado.Deducciones.IVMEmpleado);
            cmd.Parameters.AddWithValue("@BPPOEmpleado", empleado.Deducciones.BPPOEmpleado);
            cmd.Parameters.AddWithValue("@ImpuestoRenta", empleado.Deducciones.ImpuestoRenta);
            cmd.Parameters.AddWithValue("@TotalDeduccionesEmpleado", empleado.Deducciones.TotalEmpleado);
            cmd.Parameters.AddWithValue("@TotalDeduccionesPatrono", empleado.Deducciones.TotalPatrono);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Guid> InsertarPlanillaCompletaAsync(string cedulaJuridica, string periodo, DateTime fechaGeneracion, List<ResultadoEmpleadoModel> empleados, string tipo)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();

            try
            {
                var cmdEmpresa = new SqlCommand(@"
            INSERT INTO PlanillaDeduccionesEmpresa (IDPlanilla, CedulaEmpresa, Periodo, FechaDeCreacion, Tipo)
            OUTPUT INSERTED.IDPlanilla
            VALUES (NEWID(), @CedulaJuridica, @Periodo, @Fecha, @Tipo);", conn, transaction);

                cmdEmpresa.Parameters.AddWithValue("@CedulaJuridica", cedulaJuridica);
                cmdEmpresa.Parameters.AddWithValue("@Periodo", periodo);
                cmdEmpresa.Parameters.AddWithValue("@Fecha", fechaGeneracion);
                cmdEmpresa.Parameters.AddWithValue("@Tipo", tipo);

                var idPlanilla = (Guid)(await cmdEmpresa.ExecuteScalarAsync())!;

                decimal totalSEM = 0, totalIVM = 0, totalBPPO = 0, totalAsignaciones = 0, totalIMAS = 0, totalINA = 0, totalOPC = 0, totalFCL = 0, totalINS = 0, totalSalarios = 0, totalBeneficios = 0;

                foreach (var emp in empleados)
                {
                    var cmdEmp = new SqlCommand(@"
                INSERT INTO PlanillaMensualEmpleado (
                    IDPlanilla, CedulaEmpleado, SalarioBruto,
                    SEMEmpleado, IVEMEmpleado, BPPOEmpleado,
                    ImpuestoRenta, TotalDeduccionesEmpleado, TotalDeduccionesPatrono, BeneficioMonto1,
                    BeneficioNombre1, BeneficioMonto2, BeneficioNombre2, BeneficioMonto3, BeneficioNombre3, TotalDeduccionesBeneficios)
                VALUES (
                    @IDPlanilla, @CedulaEmpleado, @SalarioBruto,
                    @SEMEmpleado, @IVMEmpleado, @BPPOEmpleado,
                    @ImpuestoRenta, @TotalDeduccionesEmpleado, @TotalDeduccionesPatrono, @BeneficioMonto1,
                    @BeneficioNombre1, @BeneficioMonto2, @BeneficioNombre2, @BeneficioMonto3, @BeneficioNombre3, @TotalDeduccionesBeneficios);", conn, transaction);

                    cmdEmp.Parameters.AddWithValue("@IDPlanilla", idPlanilla);
                    cmdEmp.Parameters.AddWithValue("@CedulaEmpleado", emp.CedulaEmpleado);
                    cmdEmp.Parameters.AddWithValue("@SalarioBruto", emp.SalarioBruto);
                    cmdEmp.Parameters.AddWithValue("@SEMEmpleado", emp.Deducciones.SEMEmpleado);
                    cmdEmp.Parameters.AddWithValue("@IVMEmpleado", emp.Deducciones.IVMEmpleado);
                    cmdEmp.Parameters.AddWithValue("@BPPOEmpleado", emp.Deducciones.BPPOEmpleado);
                    cmdEmp.Parameters.AddWithValue("@ImpuestoRenta", emp.Deducciones.ImpuestoRenta);
                    cmdEmp.Parameters.AddWithValue("@TotalDeduccionesEmpleado", emp.Deducciones.TotalEmpleado);
                    cmdEmp.Parameters.AddWithValue("@TotalDeduccionesPatrono", emp.Deducciones.TotalPatrono);
                    cmdEmp.Parameters.AddWithValue("@TotalDeduccionesBeneficios", emp.Beneficios.Total);
                    cmdEmp.Parameters.AddWithValue("@BeneficioMonto1", emp.Beneficios.DeduccionesCalculadas[0].MontoReducido);
                    cmdEmp.Parameters.AddWithValue("@BeneficioNombre1", emp.Beneficios.DeduccionesCalculadas[0].NombreBeneficio);
                    cmdEmp.Parameters.AddWithValue("@BeneficioMonto2", emp.Beneficios.DeduccionesCalculadas[1].MontoReducido);
                    cmdEmp.Parameters.AddWithValue("@BeneficioNombre2", emp.Beneficios.DeduccionesCalculadas[1].NombreBeneficio);
                    cmdEmp.Parameters.AddWithValue("@BeneficioMonto3", emp.Beneficios.DeduccionesCalculadas[2].MontoReducido);
                    cmdEmp.Parameters.AddWithValue("@BeneficioNombre3", emp.Beneficios.DeduccionesCalculadas[2].NombreBeneficio);

                    await cmdEmp.ExecuteNonQueryAsync();

                    totalSEM += emp.Deducciones.SEMPatrono;
                    totalIVM += emp.Deducciones.IVMPatrono;
                    totalBPPO += emp.Deducciones.BPOPPatrono;
                    totalIMAS += emp.Deducciones.IMASPatrono;
                    totalINA += emp.Deducciones.INAPatrono;
                    totalOPC += emp.Deducciones.OPCPatrono;
                    totalAsignaciones += emp.Deducciones.AsignacionesFamiliaresPatrono;
                    totalINS += emp.Deducciones.INSPatrono;
                    totalFCL += emp.Deducciones.FCLPatrono;
                    totalSalarios += emp.SalarioBruto;
                    totalBeneficios += emp.Beneficios.Total;
                }

                var cmdUpdate = new SqlCommand(@"
            UPDATE PlanillaDeduccionesEmpresa
            SET TotalSEMPagar = @TotalSEM,
                TotalIVMPagar = @TotalIVM,
                TotalBPOPPagar = @TotalBPPO,
                TotalAsignacionesFamiliaresPagar = @TotalAsignaciones,
                TotalIMASPagar = @TotalIMAS,
                TotalINAPagar = @TotalINA,
                TotalOPCPagar = @TotalOPC,
                TotalINSPagar = @TotalINS,
                TotalFCLPagar = @TotalFCL,
                TotalSalariosPagar = @TotalSalarios,
                TotalBeneficiosPagar = @TotalBeneficios,
                FechaDeModificacion = @Fecha
            WHERE IDPlanilla = @IDPlanilla;", conn, transaction);

                cmdUpdate.Parameters.AddWithValue("@TotalSEM", totalSEM);
                cmdUpdate.Parameters.AddWithValue("@TotalIVM", totalIVM);
                cmdUpdate.Parameters.AddWithValue("@TotalBPPO", totalBPPO);
                cmdUpdate.Parameters.AddWithValue("@TotalAsignaciones", totalAsignaciones);
                cmdUpdate.Parameters.AddWithValue("@TotalIMAS", totalIMAS);
                cmdUpdate.Parameters.AddWithValue("@TotalINA", totalINA);
                cmdUpdate.Parameters.AddWithValue("@TotalOPC", totalOPC);
                cmdUpdate.Parameters.AddWithValue("@TotalINS", totalINS);
                cmdUpdate.Parameters.AddWithValue("@TotalFCL", totalFCL);
                cmdUpdate.Parameters.AddWithValue("@TotalSalarios", totalSalarios);
                cmdUpdate.Parameters.AddWithValue("@TotalBeneficios", totalBeneficios);
                cmdUpdate.Parameters.AddWithValue("@Fecha", fechaGeneracion);
                cmdUpdate.Parameters.AddWithValue("@IDPlanilla", idPlanilla);

                await cmdUpdate.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
                return idPlanilla;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> ExistePeriodoAsync(string cedulaJuridica, string periodo)
        {
            const string query = @"
                SELECT COUNT(*) 
                FROM PlanillaDeduccionesEmpresa 
                WHERE CedulaEmpresa = @CedulaJuridica AND Periodo = @Periodo";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CedulaJuridica", cedulaJuridica);
            cmd.Parameters.AddWithValue("@Periodo", periodo);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            int count = result != null ? Convert.ToInt32(result) : 0;
            return count > 0;
        }

        public async Task<string> GetTipoDePagoAsync(string cedulaJuridica)
        {
            const string query = @"
                SELECT TipoDePago 
                FROM Empresa 
                WHERE CedulaJuridica = @CedulaJuridica";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CedulaJuridica", cedulaJuridica);

            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            if (result != null && result != DBNull.Value)
            {
                return result.ToString()!;
            }

            throw new InvalidOperationException("No se encontró la empresa con esa cédula jurídica.");
        }
    }
}
