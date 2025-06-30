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
                ImpuestoRenta, TotalDeduccionesEmpleado, TotalDeduccionesPatrono, FechaGeneracion)
                VALUES (
                @IDPlanilla, @CedulaEmpleado, @SalarioBruto,
                @SEMEmpleado, @IVMEmpleado, @BPPOEmpleado,
                @ImpuestoRenta, @TotalDeduccionesEmpleado, @TotalDeduccionesPatrono, @FechaGeneracion);";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IDPlanilla", idPlanilla);
            cmd.Parameters.AddWithValue("@CedulaEmpleado", empleado.CedulaEmpleado);
            cmd.Parameters.AddWithValue("@SalarioBruto", empleado.SalarioBruto);
            cmd.Parameters.AddWithValue("@SEMEmpleado", empleado.Deducciones.SEMEmpleado);
            cmd.Parameters.AddWithValue("@IVMEmpleado", empleado.Deducciones.IVMEmpleado);
            cmd.Parameters.AddWithValue("@BPPOEmpleado", empleado.Deducciones.BPPOEmpleado);
            cmd.Parameters.AddWithValue("@ImpuestoRenta", empleado.Deducciones.ImpuestoRenta);
            cmd.Parameters.AddWithValue("@FechaGeneracion", DateTime.Now);
            cmd.Parameters.AddWithValue("@TotalDeduccionesEmpleado", empleado.Deducciones.TotalEmpleado);
            cmd.Parameters.AddWithValue("@TotalDeduccionesPatrono", empleado.Deducciones.TotalPatrono);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
