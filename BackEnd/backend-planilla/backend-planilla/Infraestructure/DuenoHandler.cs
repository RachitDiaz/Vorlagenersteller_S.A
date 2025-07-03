using backend_planilla.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace backend_planilla.Handlers
{
    public class DuenoHandler
    {
        private SqlConnection _conexion;
        private string _rutaConexion;
        private readonly PasswordHasher<UsuarioModel> _passwordHasher = new PasswordHasher<UsuarioModel>();

        public DuenoHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _rutaConexion = builder.Configuration.GetConnectionString("piTestContext");
            _conexion = new SqlConnection(_rutaConexion);
        }

        public bool CrearDueno(DuenoModel dueno)
        {
            bool exito;

            exito = CrearPersona(dueno.Persona);
            if (!exito) return false;

            exito = CrearUsuario(dueno.Persona);
            if (!exito) return false;

            exito = InsertarDueno(dueno.Persona.Cedula);
            if (!exito) return false;

            if (!string.IsNullOrEmpty(dueno.Telefono))
                InsertarTelefono(dueno.Persona.Cedula, dueno.Telefono);

            if (!string.IsNullOrEmpty(dueno.Direccion))
                InsertarDireccion(dueno.Persona.Cedula, dueno.Direccion);

            return true;
        }

        public string ObtenerCedulaDueno(string correo)
        {
            string cedulaEmpresa = "";
            var consulta = @"SELECT Dueno.Cedula
                            FROM Dueno
                            JOIN Usuario ON Usuario.Cedula = Dueno.Cedula
                            WHERE Usuario.Correo = @Correo;";
            var comandoParaConsulta = new SqlCommand(consulta, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@Correo", correo);

            try
            {
                _conexion.Open();
                var reader = comandoParaConsulta.ExecuteReader();
                if (reader.Read())
                {
                    cedulaEmpresa = reader["Cedula"].ToString();
                }
            }
            finally
            {
                if (_conexion.State != ConnectionState.Closed) { _conexion.Close(); }
            }

            return cedulaEmpresa;
        }

        private bool CrearPersona(PersonaModel persona)
        {
            var exito = false;
            try
            {
                var consulta = @"INSERT INTO Persona(Cedula, Nombre, Apellido1, Apellido2, Genero)
                                 VALUES(@Cedula, @Nombre, @Apellido1, @Apellido2, @Genero)";
                var comando = new SqlCommand(consulta, _conexion);
                comando.Parameters.AddWithValue("@Cedula", persona.Cedula);
                comando.Parameters.AddWithValue("@Nombre", persona.Nombre);
                comando.Parameters.AddWithValue("@Apellido1", persona.Apellido1 ?? "");
                comando.Parameters.AddWithValue("@Apellido2", persona.Apellido2 ?? "");
                comando.Parameters.AddWithValue("@Genero", persona.Genero ?? "");
                _conexion.Open();
                exito = comando.ExecuteNonQuery() >= 1;
                _conexion.Close();
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
            return exito;
        }

        private bool CrearUsuario(PersonaModel persona)
        {
            var exito = false;
            try
            {
                var consulta = @"INSERT INTO Usuario(Cedula, Correo, Contrasena)
                                 VALUES(@Cedula, @Correo, @Contrasena)";
                var comando = new SqlCommand(consulta, _conexion);
                persona.Usuario.Contrasena = _passwordHasher.HashPassword(persona.Usuario, persona.Usuario.Contrasena);
                comando.Parameters.AddWithValue("@Cedula", persona.Cedula);
                comando.Parameters.AddWithValue("@Correo", persona.Usuario.Correo);
                comando.Parameters.AddWithValue("@Contrasena", persona.Usuario.Contrasena);
                _conexion.Open();
                exito = comando.ExecuteNonQuery() >= 1;
                _conexion.Close();
            }
            catch(Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
            return exito;
        }

        private bool InsertarDueno(string cedula)
        {
            var exito = false;
            try
            {
                var consulta = @"INSERT INTO Dueno(Cedula) VALUES(@Cedula)";
                var comando = new SqlCommand(consulta, _conexion);
                comando.Parameters.AddWithValue("@Cedula", cedula);
                _conexion.Open();
                exito = comando.ExecuteNonQuery() >= 1;
                _conexion.Close();
            }
            catch (Exception ex) 
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
            return exito;
        }

        private void InsertarTelefono(string cedula, string telefono)
        {
            try
            {
                var consulta = @"INSERT INTO TelefonosPersona(Cedula, Telefono)
                                 VALUES(@Cedula, @Telefono)";
                var comando = new SqlCommand(consulta, _conexion);
                comando.Parameters.AddWithValue("@Cedula", cedula);
                comando.Parameters.AddWithValue("@Telefono", telefono);
                _conexion.Open();
                comando.ExecuteNonQuery();
                _conexion.Close();
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
        }

        private void InsertarDireccion(string cedula, string otrasSenas)
        {
            try
            {
                var consulta = @"INSERT INTO DireccionesPersona(Cedula, Provincia, Canton, Distrito, OtrasSenas)
                                 VALUES(@Cedula, '', '', '', @OtrasSenas)";
                var comando = new SqlCommand(consulta, _conexion);
                comando.Parameters.AddWithValue("@Cedula", cedula);
                comando.Parameters.AddWithValue("@OtrasSenas", otrasSenas);
                _conexion.Open();
                comando.ExecuteNonQuery();
                _conexion.Close();
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
        }
    }
}
