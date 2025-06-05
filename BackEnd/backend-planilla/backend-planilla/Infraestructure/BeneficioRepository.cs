using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using backend_planilla.Domain;

namespace backend_planilla.Infraestructure
{
    public class BeneficioRepository : IBeneficioRepository
    {
        private SqlConnection _conexion;
        private string _rutaConexion;

        public BeneficioRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _rutaConexion = builder.Configuration.GetConnectionString("piTestContext");
            _conexion = new SqlConnection(_rutaConexion);
        }

        public bool ActualizarBeneficiosEmpleado(string cedulaEmpleado, List<int> beneficios)
        {
            string beneficiosJson = JsonSerializer.Serialize(beneficios.Select(id => new { id }));
            string query = "EXEC ActualizarBeneficiosEmpleado @CedulaEmpleado, @ListaBeneficios";
            SqlCommand command = new SqlCommand(query, _conexion);

            command.Parameters.AddWithValue("@CedulaEmpleado", cedulaEmpleado);
            command.Parameters.AddWithValue("@ListaBeneficios", beneficiosJson);

            _conexion.Open();
            bool exito = command.ExecuteNonQuery() >= 1;
            _conexion.Close();

            return exito;
        }

        public List<BeneficioSimpleModel> ObtenerBeneficiosParaEmpleado(string correo)
        {
            var beneficios = new List<BeneficioSimpleModel>();
            var query = "SELECT ID, Nombre FROM dbo.FnObtenerBeneficiosParaEmpleado(@CorreoUsuario)";
            var comando = new SqlCommand(query, _conexion);
            comando.Parameters.AddWithValue("@CorreoUsuario", correo);

            _conexion.Open();
            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    beneficios.Add(new BeneficioSimpleModel
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Nombre = reader["Nombre"].ToString()
                    });
                }
            }
            _conexion.Close();

            return beneficios;
        }

        public List<BeneficioSimpleModel> ObtenerBeneficiosSeleccionadosPorEmpleado(string correo)
        {
            var beneficios = new List<BeneficioSimpleModel>();
            var query = "SELECT ID, Nombre FROM dbo.FnObtenerBeneficiosSeleccionados(@CorreoUsuario)";
            var comando = new SqlCommand(query, _conexion);
            comando.Parameters.AddWithValue("@CorreoUsuario", correo);

            _conexion.Open();
            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    beneficios.Add(new BeneficioSimpleModel
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Nombre = reader["Nombre"].ToString()
                    });
                }
            }
            _conexion.Close();

            return beneficios;
        }

        public string ObtenerCedulaEmpleadoDesdeCorreo(string correo)
        {
            var consulta = @"SELECT e.CedulaEmpleado
                             FROM Empleado e
                             JOIN Usuario u ON e.CedulaEmpleado = u.Cedula
                             WHERE u.Correo = @Correo";

            var comando = new SqlCommand(consulta, _conexion);
            comando.Parameters.AddWithValue("@Correo", correo);

            _conexion.Open();
            var reader = comando.ExecuteReader();

            string cedula = null;
            if (reader.Read())
                cedula = reader["CedulaEmpleado"].ToString();

            _conexion.Close();
            return cedula;
        }
    }
}
