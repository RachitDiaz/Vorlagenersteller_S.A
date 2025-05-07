using backend_planilla.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
namespace backend_planilla.Handlers
{
    public class EmpleadoHandler
    {
        private SqlConnection _conexion;
        private string _rutaConexion;
        private readonly PasswordHasher<UsuarioModel> _passwordHasher = new PasswordHasher<UsuarioModel>();
        public EmpleadoHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _rutaConexion =
            builder.Configuration.GetConnectionString("piTestContext");
            _conexion = new SqlConnection(_rutaConexion);
        }
        private DataTable CrearTablaConsulta(string consulta, string cedulaEmpresa)
        {
            SqlCommand comandoParaConsulta = new
            SqlCommand(consulta, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@cedulaEmpresa", cedulaEmpresa);
            SqlDataAdapter adaptadorParaTabla = new
            SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            _conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            _conexion.Close();
            return consultaFormatoTabla;
        }
        public List<EmpleadoModel> ObtenerEmpleados(string correo)
        {
            string cedulaEmpresa = ObtenerCedulaJuridica(correo);
            List<EmpleadoModel> empleados = new List<EmpleadoModel>();
            string consulta = @"SELECT p.Nombre, p.Apellido1, p.Apellido2, p.Cedula,
                                       e.Banco, e.SalarioBruto, e.TipoContrato
                                FROM Empleado e
                                INNER JOIN Persona p ON e.CedulaEmpleado = p.Cedula
                                WHERE e.CedulaEmpresa = @cedulaEmpresa;";
            DataTable tablaResultado = CrearTablaConsulta(consulta, cedulaEmpresa);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                empleados.Add(
                new EmpleadoModel
                {
                    CedulaEmpleado = Convert.ToString(columna["Cedula"]),
                    CedulaEmpresa = cedulaEmpresa,
                    Nombre = Convert.ToString(columna["Nombre"]),
                    Apellido1 = Convert.ToString(columna["Apellido1"]),
                    Apellido2 = Convert.ToString(columna["Apellido2"]),
                    Banco = Convert.ToString(columna["Banco"]),
                    SalarioBruto = Convert.ToDecimal(columna["SalarioBruto"]),
                    TipoContrato = Convert.ToString(columna["TipoContrato"])
                });
                Console.WriteLine($"{Convert.ToString(columna["Cedula"])}");
            }
            return empleados;
        }

        public bool CrearEmpleado(PersonaModel persona, EmpleadoModel empleado, string correo)
        {
            bool exito = false;
            Console.WriteLine($"Buscando cedula empresa");
            string cedulaEmpresa = ObtenerCedulaJuridica(correo);
            Console.WriteLine($"Buscando ID usuario");
            int IDUsuario = ObtenerIdUsuario(correo);
            Console.WriteLine($"CedulaEmpresa: {cedulaEmpresa} IDUsuario:{IDUsuario}");
            if (cedulaEmpresa != "")
            {
                exito = CrearPersona(persona);
                if (!exito) return exito;
                exito = CrearUsuario(persona);
                if (!exito) return exito;
                var consulta = @"INSERT INTO Empleado(CedulaEmpleado, CedulaEmpresa, Banco, SalarioBruto, 
                                                      TipoContrato, UsuarioCreador, UltimoEnModificar)
                                 VALUES(@CedulaEmpleado, @CedulaEmpresa, @Banco, @SalarioBruto,
                                        @TipoContrato, @UsuarioCreador, @UltimoEnModificar);";
                var comandoParaConsulta = new SqlCommand(consulta, _conexion);

                comandoParaConsulta.Parameters.AddWithValue("@CedulaEmpleado", empleado.CedulaEmpleado);
                comandoParaConsulta.Parameters.AddWithValue("@CedulaEmpresa", cedulaEmpresa);
                comandoParaConsulta.Parameters.AddWithValue("@Banco", empleado.Banco);
                comandoParaConsulta.Parameters.AddWithValue("@SalarioBruto", empleado.SalarioBruto);
                comandoParaConsulta.Parameters.AddWithValue("@TipoContrato", empleado.TipoContrato);
                comandoParaConsulta.Parameters.AddWithValue("@UsuarioCreador", IDUsuario);
                comandoParaConsulta.Parameters.AddWithValue("@UltimoEnModificar", IDUsuario);
                Console.WriteLine($"Tratando de cear empleado con la siguiente información\n" +
                    $"CedulaEmpleado: {empleado.CedulaEmpleado}\n" +
                    $"CedulaEmpresa: {cedulaEmpresa}\n" +
                    $"Banco: {empleado.Banco}\n" +
                    $"SalarioBruto: {empleado.SalarioBruto}\n" +
                    $"TipoContrato: {empleado.TipoContrato}\n" +
                    $"UsuarioCreador: {IDUsuario}\n" +
                    $"UltimoEnModificar: {IDUsuario}\n");
                _conexion.Open();
                exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
                _conexion.Close();
            }
            return exito;
        }

        public string ObtenerCedulaJuridica(string correo)
        {
            var consulta = @"SELECT e.CedulaJuridica
                             FROM Usuario u
                             JOIN Dueno d ON u.Cedula = d.Cedula
                             JOIN Empresa e ON d.Cedula = e.CedulaDueno
                             WHERE u.Correo = @Correo;";
            var comandoParaConsulta = new SqlCommand(consulta, _conexion);

            comandoParaConsulta.Parameters.AddWithValue("@Correo", correo);

            _conexion.Open();
            var reader = comandoParaConsulta.ExecuteReader();
            string cedulaEmpresa = "";

            if (reader.Read())
            {
                cedulaEmpresa = reader["CedulaJuridica"].ToString();
            }
            _conexion.Close();
            return cedulaEmpresa;
        }

        public int ObtenerIdUsuario(string correo)
        {
            var consulta = @"SELECT ID FROM Usuario
                             WHERE Correo = @CorreoUsuario;";
            var comandoParaConsulta = new SqlCommand(consulta, _conexion);

            comandoParaConsulta.Parameters.AddWithValue("@CorreoUsuario", correo);

            _conexion.Open();
            var reader = comandoParaConsulta.ExecuteReader();
            int IDUsuario = -1;

            if (reader.Read())
            {
                IDUsuario = Convert.ToInt32(reader["ID"]);
            }
            _conexion.Close();
            return IDUsuario;
        }

        public bool CrearPersona(PersonaModel persona)
        {
            var consulta = @"INSERT INTO Persona(Cedula, Nombre, Apellido1, Apellido2, Genero)
                             VALUES(@Cedula, @Nombre, @Apellido1, @Apellido2, @Genero);";
            var comandoParaConsulta = new SqlCommand(consulta, _conexion);

            comandoParaConsulta.Parameters.AddWithValue("@Cedula", persona.Cedula);
            comandoParaConsulta.Parameters.AddWithValue("@Nombre", persona.Nombre);
            comandoParaConsulta.Parameters.AddWithValue("@Apellido1", persona.Apellido1);
            comandoParaConsulta.Parameters.AddWithValue("@Apellido2", persona.Apellido2);
            comandoParaConsulta.Parameters.AddWithValue("@Genero", persona.Genero);
            _conexion.Open();
            var exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            _conexion.Close();
            return exito;
        }

        public bool CrearUsuario(PersonaModel persona)
        {
            var consulta = @"INSERT INTO Usuario(Cedula, Correo, Contrasena)
                             VALUES(@Cedula, @Correo, @Contrasena);";
            var comandoParaConsulta = new SqlCommand(consulta, _conexion);

            comandoParaConsulta.Parameters.AddWithValue("@Cedula", persona.Cedula);
            comandoParaConsulta.Parameters.AddWithValue("@Correo", persona.Usuario.Correo);
            persona.Usuario.Contrasena = _passwordHasher.HashPassword(persona.Usuario, persona.Usuario.Contrasena);
            comandoParaConsulta.Parameters.AddWithValue("@Contrasena", persona.Usuario.Contrasena);
            Console.WriteLine($"Tratando de cear usuario con la siguiente información\n" +
                $"Cedula: {persona.Cedula}" +
                $"Correo: {persona.Usuario.Correo}" +
                $"Contrasena: {persona.Usuario.Contrasena}");
            _conexion.Open();
            var exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            _conexion.Close();
            return exito;
        }

    }
}