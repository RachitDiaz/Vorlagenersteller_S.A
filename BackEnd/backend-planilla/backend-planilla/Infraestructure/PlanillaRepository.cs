using Microsoft.Data.SqlClient;
using backend_planilla.Domain;
using System.Data;

namespace backend_planilla.Infraestructure
{
    public class PlanillaRepository : IPlanillaRepository
    {
        private readonly string _connectionString;

        public PlanillaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<PlanillaMensualEmpleadoModel>> ObtenerEmpleadosPorEmpresaAsync(string cedulaEmpresa)
        {
            var empleados = new List<PlanillaMensualEmpleadoModel>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ObtenerEmpleadosPorEmpresa", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@CedulaEmpresa", cedulaEmpresa);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                empleados.Add(new PlanillaMensualEmpleadoModel
                {
                    CedulaEmpleado = reader["CedulaEmpleado"].ToString(),
                    SalarioBruto = Convert.ToDecimal(reader["SalarioBruto"]),
                    FechaInicioTrabajo = Convert.ToDateTime(reader["FechaInicioTrabajo"])
                });
            }

            return empleados;
        }

        public async Task<List<decimal>> ObtenerMontosBeneficiosEmpleadoAsync(string cedulaEmpleado)
        {
            var beneficios = new List<decimal>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("ObtenerMontosBeneficiosEmpleado", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@CedulaEmpleado", cedulaEmpleado);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                beneficios.Add(Convert.ToDecimal(reader["Monto"]));
            }

            return beneficios;
        }

        public async Task<bool> InsertarPlanillaEmpleadoAsync(int idPlanilla, string cedulaEmpleado, decimal salarioBruto, DeduccionesPlanillaModel d, List<decimal> beneficios, decimal totalEmpleado, decimal totalPatrono)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("InsertarPlanillaMensualEmpleado", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@IDPlanilla", idPlanilla);
            command.Parameters.AddWithValue("@CedulaEmpleado", cedulaEmpleado);
            command.Parameters.AddWithValue("@SalarioBruto", salarioBruto);
            command.Parameters.AddWithValue("@SEMEmpleado", d.SEMEmpleado);
            command.Parameters.AddWithValue("@SEMPatrono", d.SEMPatrono);
            command.Parameters.AddWithValue("@IVMEmpleado", d.IVMEmpleado);
            command.Parameters.AddWithValue("@IVMPatrono", d.IVMPatrono);
            command.Parameters.AddWithValue("@LPTEmpleado", d.LPTEmpleado);
            command.Parameters.AddWithValue("@LPTPatrono", d.LPTPatrono);
            command.Parameters.AddWithValue("@ImpuestoRenta", d.ImpuestoRenta);
            command.Parameters.AddWithValue("@Beneficio1", beneficios.ElementAtOrDefault(0));
            command.Parameters.AddWithValue("@Beneficio2", beneficios.ElementAtOrDefault(1));
            command.Parameters.AddWithValue("@Beneficio3", beneficios.ElementAtOrDefault(2));
            command.Parameters.AddWithValue("@TotalDeduccionesEmpleado", totalEmpleado);
            command.Parameters.AddWithValue("@TotalDeduccionesPatrono", totalPatrono);

            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> InsertarResumenEmpresaAsync(int idPlanilla, string cedulaEmpresa, string periodo,
            decimal totalSEMPagar, decimal totalSEMDeducir, decimal totalIVMPagar, decimal totalIVMDeducir,
            decimal totalLPTPagar, decimal totalLPTDeducir, decimal totalRenta, decimal totalBeneficios)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("InsertarPlanillaMensualEmpresa", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@IDPlanilla", idPlanilla);
            command.Parameters.AddWithValue("@CedulaEmpresa", cedulaEmpresa);
            command.Parameters.AddWithValue("@PeriodoMes", periodo);
            command.Parameters.AddWithValue("@TotalSEMPagar", totalSEMPagar);
            command.Parameters.AddWithValue("@TotalSEMDeducir", totalSEMDeducir);
            command.Parameters.AddWithValue("@TotalIVMPagar", totalIVMPagar);
            command.Parameters.AddWithValue("@TotalIVMDeducir", totalIVMDeducir);
            command.Parameters.AddWithValue("@TotalLPTPagar", totalLPTPagar);
            command.Parameters.AddWithValue("@TotalLPTDeducir", totalLPTDeducir);
            command.Parameters.AddWithValue("@TotalRentaDeducir", totalRenta);
            command.Parameters.AddWithValue("@TotalBeneficiosDeducir", totalBeneficios);

            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync() > 0;
        }
    }
}
