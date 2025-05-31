using System.Data;
using backend_planilla.Domain;
using Microsoft.Data.SqlClient;

namespace backend_planilla.Infraestructure
{
    public class BeneficiosRepository : IBeneficiosRepository
    {
        private SqlConnection _conexion;
        private string _rutaConexion;

        public BeneficiosRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _rutaConexion =
            builder.Configuration.GetConnectionString("piTestContext");
            _conexion = new SqlConnection(_rutaConexion);
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

        private DataTable CrearTablaConsulta(string consulta, string cedulaEmpresa)
        {
            SqlCommand comandoParaConsulta = new(consulta, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@CedulaJuridica", cedulaEmpresa);
            SqlDataAdapter adaptadorParaTabla = new(comandoParaConsulta);
            DataTable consultaFormatoTabla = new();
            _conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            _conexion.Close();
            return consultaFormatoTabla;
        }

        public List<BeneficioModel> ObtenerBeneficios(string cedulaEmpresa)
        {
            #pragma warning disable IDE0028
            List<BeneficioModel> beneficios = new();
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
                    CantidadParametros = Convert.ToInt32(columna["CantidadParametros"])
                });
            }
            #pragma warning restore IDE0028
            Console.WriteLine(beneficios);
            return beneficios;
        }

        public int CrearBeneficio(BeneficioModel beneficio, string cedulaEmpresa, int IDUsuario)
        {
            int idBeneficio = -1;
            var consulta = @"INSERT INTO [dbo].[Beneficio] ([Nombre], [Tipo] ,[Descripcion], [ServicioExterno],
                                            [MesesMinimos], [CantidadParametros], [UsuarioCrea], [UsuarioModifica])
                                VALUES(@Nombre, @Tipo , @Descripcion, @ServicioExterno,
                                    @MesesMinimos, @CantidadParametros, @UsuarioCrea, @UsuarioModifica)
                                SELECT SCOPE_IDENTITY() AS IDBeneficio;";
#pragma warning disable IDE0063
            try
            {   
                using (var comandoParaConsulta = new SqlCommand(consulta, _conexion))
                {
                    comandoParaConsulta.Parameters.AddWithValue("@Nombre", beneficio.Nombre);
                    comandoParaConsulta.Parameters.AddWithValue("@Tipo", beneficio.Tipo);
                    comandoParaConsulta.Parameters.AddWithValue("@Descripcion", beneficio.Descripcion);
                    comandoParaConsulta.Parameters.AddWithValue("@ServicioExterno", beneficio.ServicioExterno);
                    comandoParaConsulta.Parameters.AddWithValue("@MesesMinimos", beneficio.MesesMinimos);
                    comandoParaConsulta.Parameters.AddWithValue("@CantidadParametros", beneficio.CantidadParametros);
                    comandoParaConsulta.Parameters.AddWithValue("@UsuarioCrea", IDUsuario);
                    comandoParaConsulta.Parameters.AddWithValue("@UsuarioModifica", IDUsuario);

                    _conexion.Open();

                    using (var reader = comandoParaConsulta.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idBeneficio = Convert.ToInt32(reader["IDBeneficio"]);
                            Console.WriteLine($"Ingresado con éxito en beneficio: {idBeneficio}");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                throw new Exception("Error al insertar beneficio en la base de datos");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("Ocurrió un error inesperado en BeneficiosRepository.CrearBeneficio");
            }
            finally
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
#pragma warning restore IDE0063
            return idBeneficio;
        }

        public bool CrearRelacionEmpresaBeneficio(string cedulaEmpresa, int idBeneficio)
        {
            try
            {
                var consulta = @"INSERT INTO [dbo].[EmpresaOfreceBeneficio] ([IDBeneficio], [CedulaEmpresa])
                         VALUES(@IDBeneficio, @CedulaEmpresa);";
                var comandoParaConsulta = new SqlCommand(consulta, _conexion);

                comandoParaConsulta.Parameters.AddWithValue("@IDBeneficio", idBeneficio);
                comandoParaConsulta.Parameters.AddWithValue("@CedulaEmpresa", cedulaEmpresa);
                Console.WriteLine($"Creando RelacionEmpresaBeneficio {cedulaEmpresa} {idBeneficio}");
                _conexion.Open();
                bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
                return exito;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creando relación: {ex.Message}");
                return false;
            }
            finally
            {
                _conexion.Close();
            }
        }

    }
}
