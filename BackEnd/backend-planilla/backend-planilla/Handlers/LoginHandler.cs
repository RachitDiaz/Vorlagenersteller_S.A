using backend_planilla.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace backend_planilla.Handlers
{
    public class LoginHandler
    {
        private SqlConnection _conexion;
        private string _rutaConexion;
        public LoginHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _rutaConexion =
            builder.Configuration.GetConnectionString("piTestContext");
            _conexion = new SqlConnection(_rutaConexion);
        }

        private DataTable CrearTablaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new
            SqlCommand(consulta, _conexion);
            SqlDataAdapter adaptadorParaTabla = new
            SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            _conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            _conexion.Close();
            return consultaFormatoTabla;
        }

        public (string? CorreoUsuario, bool EsDueno) ValidarCredenciales(string correo, string contrasena)
        {
            var consulta = @"
                    SELECT 
                        u.Correo,
                        CASE WHEN d.Cedula IS NOT NULL THEN 1 ELSE 0 END AS EsDueno
                    FROM Usuario u
                    LEFT JOIN Dueno d ON u.Cedula = d.Cedula
                    WHERE u.Correo = @correo AND u.Contrasena = @contrasena;";
            var comandoParaConsulta = new SqlCommand(consulta, _conexion);

            comandoParaConsulta.Parameters.AddWithValue("@correo", correo);
            comandoParaConsulta.Parameters.AddWithValue("@contrasena", contrasena);

            _conexion.Open();
            var reader = comandoParaConsulta.ExecuteReader();

            if (reader.Read())
            {
                string correoUsuario = reader["Correo"].ToString();
                bool esDueno = (int)reader["EsDueno"] == 1;
                _conexion.Close();
                return (correoUsuario, esDueno);
            }

            _conexion.Close();
            return (null, false); // Usuario no encontrado
        }
    }
}
