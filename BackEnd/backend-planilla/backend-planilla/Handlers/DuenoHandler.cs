using backend_planilla.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;

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

        private bool CrearPersona(PersonaModel persona)
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
            var exito = comando.ExecuteNonQuery() >= 1;
            _conexion.Close();
            return exito;
        }

        private bool CrearUsuario(PersonaModel persona)
        {
            var consulta = @"INSERT INTO Usuario(Cedula, Correo, Contrasena)
                             VALUES(@Cedula, @Correo, @Contrasena)";
            var comando = new SqlCommand(consulta, _conexion);
            persona.Usuario.Contrasena = _passwordHasher.HashPassword(persona.Usuario, persona.Usuario.Contrasena);
            comando.Parameters.AddWithValue("@Cedula", persona.Cedula);
            comando.Parameters.AddWithValue("@Correo", persona.Usuario.Correo);
            comando.Parameters.AddWithValue("@Contrasena", persona.Usuario.Contrasena);
            _conexion.Open();
            var exito = comando.ExecuteNonQuery() >= 1;
            _conexion.Close();
            return exito;
        }

        private bool InsertarDueno(string cedula)
        {
            var consulta = @"INSERT INTO Dueno(Cedula) VALUES(@Cedula)";
            var comando = new SqlCommand(consulta, _conexion);
            comando.Parameters.AddWithValue("@Cedula", cedula);
            _conexion.Open();
            var exito = comando.ExecuteNonQuery() >= 1;
            _conexion.Close();
            return exito;
        }

        private void InsertarTelefono(string cedula, string telefono)
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

        private void InsertarDireccion(string cedula, string otrasSenas)
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
    }
}
