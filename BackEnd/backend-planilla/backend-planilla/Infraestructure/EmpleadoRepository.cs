using backend_planilla.Models;
using backend_planilla.Domain;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using backend_planilla.Infraestructure;
using System.Data.Common;
using System.Transactions;
namespace backend_planilla.Handlers
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private SqlConnection _conexion;
        private string _rutaConexion;
        private readonly PasswordHasher<UsuarioModel> _passwordHasher = new PasswordHasher<UsuarioModel>();
        public EmpleadoRepository()
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
                                WHERE e.CedulaEmpresa = @cedulaEmpresa
                                AND e.Borrado = 0;";
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
            }
            return empleados;
        }

        public bool CrearEmpleado(PersonaModel persona, EmpleadoModel empleado, string correo)
        {
            bool exito = false;
            string cedulaEmpresa = ObtenerCedulaJuridica(correo);
            int IDUsuario = ObtenerIdUsuario(correo);
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

        public string ObtenerCedulaEmpleado(string correo)
        {
            string cedulaEmpresa = "";
            var consulta =  @"SELECT Empleado.CedulaEmpleado
                            FROM Usuario
                            JOIN Empleado ON Usuario.Cedula = Empleado.CedulaEmpleado
                            WHERE Usuario.Correo = @Correo;";
            var comandoParaConsulta = new SqlCommand(consulta, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@Correo", correo);

            try
            {
                _conexion.Open();
                var reader = comandoParaConsulta.ExecuteReader();
                if (reader.Read())
                {
                    cedulaEmpresa = reader["CedulaEmpleado"].ToString();
                }
            }
            finally
            {
                if (_conexion.State != ConnectionState.Closed) { _conexion.Close(); }
            }

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
            _conexion.Open();
            var exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            _conexion.Close();
            return exito;
        }

        public InfoEmpleadoModel? ObtenerInfoEmpleado(string cedulaEmpleado)
        {

            var consulta = @"Select	Persona.Cedula, Persona.Nombre, Persona.Apellido1, Persona.Apellido2,
                            Persona.Genero, Empleado.Banco, Empleado.SalarioBruto, Empleado.CedulaEmpresa,
                            Empleado.TipoContrato, Usuario.Correo, Empleado.Editable
                            From Persona
                            INNER JOIN Empleado ON Empleado.CedulaEmpleado = Persona.Cedula
                            INNER JOIN Usuario ON Usuario.Cedula = Persona.Cedula
                            WHERE Persona.Cedula = @cedulaBusqueda;";
            var comandoParaConsulta = new SqlCommand(consulta, _conexion);

            comandoParaConsulta.Parameters.AddWithValue("@cedulaBusqueda", cedulaEmpleado);

            _conexion.Open();

            var lector = comandoParaConsulta.ExecuteReader();
            InfoEmpleadoModel? infoEmpleado = null;

            if (lector.Read())
            {
                infoEmpleado = new InfoEmpleadoModel
                {
                    Empleado = new EmpleadoModel
                    {
                        CedulaEmpleado = lector["Cedula"].ToString(),
                        CedulaEmpresa = lector["CedulaEmpresa"].ToString(),
                        Nombre = lector["Nombre"].ToString(),
                        Apellido1 = lector["Apellido1"].ToString(),
                        Apellido2 = lector["Apellido2"].ToString(),
                        Banco = lector["Banco"].ToString(),
                        SalarioBruto = Convert.ToDecimal(lector["SalarioBruto"]),
                        TipoContrato = lector["TipoContrato"].ToString()
                    },
                    Genero = lector["Genero"].ToString(),
                    Correo = lector["Correo"].ToString(),
                    CedulaEditable = Convert.ToBoolean(lector["Editable"])
                };

            }
            _conexion.Close();

            return infoEmpleado;
        }

        public bool EditarInfoEmpleado(InfoEmpleadoModel datosNuevos, string cedulaEmpleado)
        {

            var consulta = @"EXECUTE EditarEmpleado @CedulaEmpleado = @@CedulaEmpleado,
						@NewCedula = @@NewCedula,
						@NewNombre = @@NewNombre,
						@NewApellido1 = @@NewApellido1,
						@NewApellido2 = @@NewApellido2,
						@NewGenero = @@NewGenero, 
						@NewBanco = @@NewBanco,
						@NewContrato = @@NewContrato, 
						@NewCorreo = @@NewCorreo,
						@NewSalario = @@NewSalario";

            var comandoParaConsulta = new SqlCommand(consulta, _conexion);

            comandoParaConsulta.Parameters.AddWithValue("@@CedulaEmpleado", cedulaEmpleado);
            comandoParaConsulta.Parameters.AddWithValue("@@NewCedula", datosNuevos.Empleado.CedulaEmpleado);
            comandoParaConsulta.Parameters.AddWithValue("@@NewNombre", datosNuevos.Empleado.Nombre);
            comandoParaConsulta.Parameters.AddWithValue("@@NewApellido1", datosNuevos.Empleado.Apellido1);
            comandoParaConsulta.Parameters.AddWithValue("@@NewApellido2", datosNuevos.Empleado.Apellido2);
            comandoParaConsulta.Parameters.AddWithValue("@@NewGenero", datosNuevos.Genero);
            comandoParaConsulta.Parameters.AddWithValue("@@NewBanco", datosNuevos.Empleado.Banco);
            comandoParaConsulta.Parameters.AddWithValue("@@NewContrato", datosNuevos.Empleado.TipoContrato);
            comandoParaConsulta.Parameters.AddWithValue("@@NewCorreo", datosNuevos.Correo);
            comandoParaConsulta.Parameters.AddWithValue("@@NewSalario", datosNuevos.Empleado.SalarioBruto);

            _conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            _conexion.Close();

            return exito;
        }


        public async Task<string> ObtenerCorreoDesdeCedula(string cedula)
        {
            var consulta = @"SELECT Correo FROM Usuario
                            WHERE Cedula  = @Cedula;";
            var comandoParaConsulta = new SqlCommand(consulta, _conexion);

            comandoParaConsulta.Parameters.AddWithValue("@Cedula", cedula);

            _conexion.Open();
            var reader = comandoParaConsulta.ExecuteReader();
            string result = "";
            if (reader.Read())
            {
                result = reader["Correo"].ToString();
            }
            _conexion.Close();
            return result;
        }
        public async Task<decimal> ObtenerSalarioBruto(string cedulaEmpleado)
        {
            var query = "SELECT SalarioBruto FROM Empleado WHERE CedulaEmpleado = @Cedula";
            using var cmd = new SqlCommand(query, _conexion);
            cmd.Parameters.AddWithValue("@Cedula", cedulaEmpleado);
            _conexion.Open();
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToDecimal(result);
        }

        public async Task<List<DeduccionBeneficioModel>> ObtenerBeneficiosEmpleado(string cedulaEmpleado)
        {
            var beneficios = new List<DeduccionBeneficioModel>();
            var query = @"SELECT B.ID, B.Nombre, B.Tipo AS Tipo
                        FROM EligeBeneficio EB
                        JOIN Beneficio B ON EB.IDBeneficio = B.ID
                        JOIN EligeBeneficio PB ON PB.IDBeneficio = B.ID
                        WHERE EB.CedulaEmpleado = @Cedula";

            using var cmd = new SqlCommand(query, _conexion);
            cmd.Parameters.AddWithValue("@Cedula", cedulaEmpleado);

            try
            {
                if (_conexion.State != ConnectionState.Open)
                    _conexion.Open();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    beneficios.Add(new DeduccionBeneficioModel
                    {
                        IDBeneficio = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Tipo = reader.GetString(2)
                    });
                }
            }
            finally
            {
                if (_conexion.State != ConnectionState.Closed)
                    _conexion.Close();
            }

            return beneficios;
        }
    

        public async Task<string> ObtenerGeneroEmpleado(string cedulaEmpleado)
        {
            var query = "SELECT Genero FROM Persona WHERE Cedula = @Cedula";
            using var cmd = new SqlCommand(query, _conexion);
            cmd.Parameters.AddWithValue("@Cedula", cedulaEmpleado);

            if (_conexion.State != ConnectionState.Open)
                _conexion.Open();

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader["Genero"].ToString()?.ToLower() switch
                {
                    "m" => "male",
                    "f" => "female",
                    _ => "male"
                };
            }

            throw new Exception("No se encontró el género del empleado.");
        }

        public async Task<string> ObtenerFechaNacimientoEmpleado(string cedulaEmpleado)
        {
            var query = "SELECT FechaNacimiento FROM Persona WHERE Cedula = @Cedula";
            using var cmd = new SqlCommand(query, _conexion);
            cmd.Parameters.AddWithValue("@Cedula", cedulaEmpleado);

            if (_conexion.State != ConnectionState.Open)
                _conexion.Open();

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader["FechaNacimiento"].ToString()?.ToLower();
            }

            throw new Exception("No se encontró la fecha de nacimiento del empleado.");
        }

        public async Task<string> ObtenerCantDependientesEmpleado(string cedulaEmpleado)
        {
            var query = "SELECT CantidadDependientes FROM Empleado WHERE CedulaEmpleado = @Cedula";
            using var cmd = new SqlCommand(query, _conexion);
            cmd.Parameters.AddWithValue("@Cedula", cedulaEmpleado);

            if (_conexion.State != ConnectionState.Open)
                _conexion.Open();

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader["CantidadDependientes"].ToString()?.ToLower();
            }

            throw new Exception("No se encontró la fecha de nacimiento del empleado.");
        }
        public string EliminarEmpleado(string cedulaEmpleado)
        {
            string correoUsuario = "";
            _conexion.Open();
            var transaccion = _conexion.BeginTransaction();
            try
            {
                var consulta = @"SELECT Correo FROM Usuario WHERE Usuario.Cedula = @cedulaEmpleado";
                var comandoParaConsulta = new SqlCommand(consulta, _conexion, transaccion);

                comandoParaConsulta.Parameters.AddWithValue("@cedulaEmpleado", cedulaEmpleado);
                var reader = comandoParaConsulta.ExecuteReader();

                if (reader.Read())
                {
                    correoUsuario = Convert.ToString(reader["Correo"]);
                }
                else
                {
                    reader.Close();
                    transaccion.Rollback();
                    throw new InvalidOperationException("No se encontró un usuario con esta cédula");
                }

                reader.Close();

                consulta = @"DELETE FROM Empleado WHERE CedulaEmpleado = @cedulaEmpleado";
                comandoParaConsulta = new SqlCommand(consulta, _conexion, transaccion);
                comandoParaConsulta.Parameters.AddWithValue("@cedulaEmpleado", cedulaEmpleado);

                if (comandoParaConsulta.ExecuteNonQuery() >= 1)
                {
                    transaccion.Commit();
                    return correoUsuario;
                }
                else
                {
                    transaccion.Rollback();
                    throw new Exception("Ocurrió un error en el delete y no se afectó ninguna fila");
                }
            }
            catch (Exception ex)
            {
                transaccion.Rollback();
                throw new Exception("Ocurrió un error en el query");
            }
            finally
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
        }
    }
}