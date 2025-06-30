using backend_planilla.Application;
using Microsoft.Data.SqlClient;
using System.Data;

namespace backend_planilla.Infraestructure
{
    public class GenerarCalculosRepository : IGenerarCalculosRepository
    {
        private readonly string _connectionString;

        public GenerarCalculosRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("piTestContext")!;
        }

        public async Task<List<(string CedulaEmpleado, decimal Salario)>> ObtenerEmpleadosConSalarioAsync(string cedulaJuridica)
        {
            var resultado = new List<(string, decimal)>();
            var query = @"SELECT CedulaEmpleado, SalarioBruto 
                          FROM Empleado 
                          WHERE LOWER(CedulaEmpresa) = LOWER(@CedulaJuridica)";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CedulaJuridica", cedulaJuridica);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                string cedula = reader.GetString(0);
                decimal salario = reader.GetDecimal(1);
                resultado.Add((cedula, salario));
            }

            return resultado;
        }
    }
}
