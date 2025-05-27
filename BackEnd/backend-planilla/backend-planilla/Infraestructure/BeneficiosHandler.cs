using backend_planilla.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace backend_planilla.Handlers
{
    public class BeneficiosHandler
    {
        private SqlConnection _conexion;
        private string _rutaConexion;
        public BeneficiosHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _rutaConexion =builder.Configuration.GetConnectionString("piTestContext");
            _conexion = new SqlConnection(_rutaConexion);
        }
        private DataTable CrearTablaConsulta(string consulta, string cedulaEmpresa)
        {
            SqlCommand comandoParaConsulta = new
            SqlCommand(consulta, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@CedulaJuridica", cedulaEmpresa);
            SqlDataAdapter adaptadorParaTabla = new
            SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            _conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            _conexion.Close();
            return consultaFormatoTabla;
        }
        public List<BeneficioModel> ObtenerBeneficios(string correo)
        {
            string cedulaEmpresa = ObtenerCedulaJuridica(correo);
            List<BeneficioModel> beneficios = new List<BeneficioModel>();
            string consulta = @"SELECT b.*
                               FROM EmpresaOfreceBeneficio eob
                               JOIN Beneficio b ON eob.IDBeneficio = b.ID
                               WHERE eob.CedulaEmpresa = @CedulaJuridica;";
            DataTable tablaResultado = CrearTablaConsulta(consulta, cedulaEmpresa);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                beneficios.Add(
                new BeneficioModel
                {
                    Nombre = Convert.ToString(columna["Nombre"]),
                    Tipo = Convert.ToString(columna["Tipo"]),
                    Descripcion = Convert.ToString(columna["Descripcion"]),
                    ServicioExterno = Convert.ToString(columna["ServicioExterno"]),
                    MesesMinimos = Convert.ToInt32(columna["MesesMinimos"]),
                    CantidadParametros = Convert.ToInt32(columna["CantidadParametros"]),
                    Id = Convert.ToInt32(columna["ID"]),
                });
            }
            Console.WriteLine(beneficios);
            return beneficios;
        }

        public bool CrearBeneficio(BeneficioModel beneficio, string correo)
        {
            bool exito = false;
            Console.WriteLine($"Buscando cedula empresa");
            string cedulaEmpresa = ObtenerCedulaJuridica(correo);
            Console.WriteLine($"Buscando ID usuario");
            int IDUsuario = ObtenerIdUsuario(correo);
            Console.WriteLine($"CedulaEmpresa: {cedulaEmpresa} IDUsuario:{IDUsuario}");
            if (cedulaEmpresa != "")
            {
                var consulta = @"INSERT INTO [dbo].[Beneficio] ([Nombre], [Tipo] ,[Descripcion], [ServicioExterno],
                                             [MesesMinimos], [CantidadParametros], [UsuarioCrea], [UsuarioModifica])
                                 VALUES(@Nombre, @Tipo , @Descripcion, @ServicioExterno,
                                        @MesesMinimos, @CantidadParametros, @UsuarioCrea, @UsuarioModifica)
                                 SELECT SCOPE_IDENTITY() AS IDBeneficio;";
                var comandoParaConsulta = new SqlCommand(consulta, _conexion);

                comandoParaConsulta.Parameters.AddWithValue("@Nombre", beneficio.Nombre);
                comandoParaConsulta.Parameters.AddWithValue("@Tipo", beneficio.Tipo);
                comandoParaConsulta.Parameters.AddWithValue("@Descripcion", beneficio.Descripcion);
                comandoParaConsulta.Parameters.AddWithValue("@ServicioExterno", beneficio.ServicioExterno);
                comandoParaConsulta.Parameters.AddWithValue("@MesesMinimos", beneficio.MesesMinimos);
                comandoParaConsulta.Parameters.AddWithValue("@CantidadParametros", beneficio.CantidadParametros);
                comandoParaConsulta.Parameters.AddWithValue("@UsuarioCrea", IDUsuario);
                comandoParaConsulta.Parameters.AddWithValue("@UsuarioModifica", IDUsuario);

                _conexion.Open();
                var reader = comandoParaConsulta.ExecuteReader();
                if (reader.Read())
                {
                    var idBeneficio = Convert.ToInt32(reader["IDBeneficio"]);
                    Console.WriteLine($"Ingresado con éxito en beneficio: {idBeneficio}");
                    reader.Close();
                    _conexion.Close();
                    exito = CrearRelacionEmpresaBeneficio(cedulaEmpresa, idBeneficio);
                }
                reader.Close();
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

        public bool CrearRelacionEmpresaBeneficio(string cedulaEmpresa, int idBeneficio)
        {
            var consulta = @"INSERT INTO [dbo].[EmpresaOfreceBeneficio] ([IDBeneficio], [CedulaEmpresa])
                                 VALUES(@IDBeneficio, @CedulaEmpresa);";
            var comandoParaConsulta = new SqlCommand(consulta, _conexion);

            comandoParaConsulta.Parameters.AddWithValue("@IDBeneficio", idBeneficio);
            comandoParaConsulta.Parameters.AddWithValue("@CedulaEmpresa", cedulaEmpresa);
            Console.WriteLine($"Creando RelacionEmpresaBeneficio {cedulaEmpresa} {idBeneficio}");
            _conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            _conexion.Close();

            return exito;
        }

    }
}