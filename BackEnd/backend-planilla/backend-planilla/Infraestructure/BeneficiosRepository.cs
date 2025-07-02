using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Transactions;
using backend_planilla.Domain;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            string cedulaEmpresa = "";
            try
            {
                var consultaDueño = @"
                                      SELECT e.CedulaJuridica
                                      FROM Usuario u
                                      JOIN Dueno d ON u.Cedula = d.Cedula
                                      JOIN Empresa e ON d.Cedula = e.CedulaDueno
                                      WHERE u.Correo = @Correo;";

                var consultaEmpleado = @"
                                         SELECT e.CedulaEmpresa
                                         FROM Usuario u
                                         JOIN Empleado e ON u.Cedula = e.CedulaEmpleado
                                         WHERE u.Correo = @Correo;";

                using (var comando = new SqlCommand(consultaDueño, _conexion))
                {
                    comando.Parameters.AddWithValue("@Correo", correo);
                    _conexion.Open();
                    var reader = comando.ExecuteReader();

                    if (reader.Read())
                    {
                        cedulaEmpresa = reader["CedulaJuridica"].ToString();
                    }
                }

                if (string.IsNullOrEmpty(cedulaEmpresa))
                {
                    using (var comando = new SqlCommand(consultaEmpleado, _conexion))
                    {
                        comando.Parameters.AddWithValue("@Correo", correo);
                        _conexion.Open();
                        var reader = comando.ExecuteReader();

                        if (reader.Read())
                        {
                            cedulaEmpresa = reader["CedulaEmpresa"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
            return cedulaEmpresa;
        }

        public int ObtenerIdUsuario(string correo)
        {
            int IDUsuario = -1;
            try
            {
                var consulta = @"SELECT ID FROM Usuario
                                 WHERE Correo = @CorreoUsuario;";
                var comandoParaConsulta = new SqlCommand(consulta, _conexion);

                comandoParaConsulta.Parameters.AddWithValue("@CorreoUsuario", correo);

                _conexion.Open();
                var reader = comandoParaConsulta.ExecuteReader();

                if (reader.Read())
                {
                    IDUsuario = Convert.ToInt32(reader["ID"]);
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
            return IDUsuario;
        }

        private DataTable CrearTablaConsulta(string consulta, string cedulaEmpresa)
        {
            DataTable consultaFormatoTabla = new();
            try
            {
                SqlCommand comandoParaConsulta = new(consulta, _conexion);
                comandoParaConsulta.Parameters.AddWithValue("@CedulaJuridica", cedulaEmpresa);
                SqlDataAdapter adaptadorParaTabla = new(comandoParaConsulta);
                _conexion.Open();
                adaptadorParaTabla.Fill(consultaFormatoTabla);
                _conexion.Close();
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
            finally
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
            }
            return consultaFormatoTabla;
        }

        public List<BeneficioModel> ObtenerBeneficios(string cedulaEmpresa)
        {
            #pragma warning disable IDE0028
            List<BeneficioModel> beneficios = new();
            try
            {
                string consulta = @"SELECT *
	                                FROM Beneficio
	                                WHERE CedulaEmpresa = @CedulaJuridica
	                                OR
	                                (CedulaEmpresa IS NULL AND Tipo = 'API');";
                DataTable tablaResultado = CrearTablaConsulta(consulta, cedulaEmpresa);
                foreach (DataRow columna in tablaResultado.Rows)
                {
                    int Id = Convert.ToInt32(columna["ID"]);
                    beneficios.Add(
                    new BeneficioModel
                    {
                        Id = Id,
                        Nombre = Convert.ToString(columna["Nombre"]),
                        Tipo = Convert.ToString(columna["Tipo"]),
                        Descripcion = Convert.ToString(columna["Descripcion"]),
                        CedulaEmpresa = Convert.ToString(columna["CedulaEmpresa"]),
                        MesesMinimos = Convert.ToInt32(columna["MesesMinimos"]),
                        CantidadParametros = Convert.ToInt32(columna["CantidadParametros"]),
                        Parametros = ObtenerParametrosBeneficio(Id)
                    });
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                throw new Exception(sqlEx.Message);
            }
            finally
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
#pragma warning restore IDE0028
            return beneficios;
        }

        public List<ParametroBeneficioModel> ObtenerParametrosBeneficio(int idBeneficio)
        {
            var parametros = new List<ParametroBeneficioModel>();
            try
            {
                var query = @"SELECT * FROM ParametrosBeneficio WHERE IDBeneficio = @IDBeneficio";
                using var command = new SqlCommand(query, _conexion);
                command.Parameters.AddWithValue("@IDBeneficio", idBeneficio);

                _conexion.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    parametros.Add(new ParametroBeneficioModel
                    {
                        IDParametro = Convert.ToInt32(reader["IDParametro"]),
                        IDBeneficio = Convert.ToInt32(reader["IDBeneficio"]),
                        Nombre = Convert.ToString(reader["Nombre"]),
                        TipoDeDatoParametro = Convert.ToString(reader["TipoDeDatoParametro"]),
                        TipoValorParametro = Convert.ToString(reader["TipoValorParametro"]),
                        ValorDelParametro = Convert.ToInt32(reader["ValorDelParametro"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
            return parametros;
        }

        public int CrearBeneficio(BeneficioModel beneficio, string cedulaEmpresa, int idUsuario)
        {
            int idBeneficio = -1;
#pragma warning disable IDE0063
            try
            {
                var consulta = @"INSERT INTO [dbo].[Beneficio] ([Nombre], [Tipo] ,[Descripcion],
                                                [MesesMinimos], [CantidadParametros], [UsuarioCrea],
                                                [UsuarioModifica], [CedulaEmpresa])
                                    VALUES(@Nombre, @Tipo , @Descripcion,
                                        @MesesMinimos, @CantidadParametros,
                                        @UsuarioCrea, @UsuarioModifica, @CedulaEmpresa)
                                    SELECT SCOPE_IDENTITY() AS IDBeneficio;";
                using (var comandoParaConsulta = new SqlCommand(consulta, _conexion))
                {
                    comandoParaConsulta.Parameters.AddWithValue("@Nombre", beneficio.Nombre);
                    comandoParaConsulta.Parameters.AddWithValue("@Tipo", beneficio.Tipo);
                    comandoParaConsulta.Parameters.AddWithValue("@Descripcion", beneficio.Descripcion);
                    comandoParaConsulta.Parameters.AddWithValue("@MesesMinimos", beneficio.MesesMinimos);
                    comandoParaConsulta.Parameters.AddWithValue("@CantidadParametros", beneficio.CantidadParametros);
                    comandoParaConsulta.Parameters.AddWithValue("@UsuarioCrea", idUsuario);
                    comandoParaConsulta.Parameters.AddWithValue("@UsuarioModifica", idUsuario);
                    comandoParaConsulta.Parameters.AddWithValue("@CedulaEmpresa", cedulaEmpresa);

                    _conexion.Open();

                    using (var reader = comandoParaConsulta.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idBeneficio = Convert.ToInt32(reader["IDBeneficio"]);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                {
                    throw new Exception("El beneficio ya existe en la base de datos.");
                }
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

        public bool CrearParametros(List<ParametroBeneficioModel> parametros, int idBeneficio)
        {
            bool exito = false;
            try
            {
                string query = @"INSERT INTO [dbo].[ParametrosBeneficio]
                                     (IDBeneficio, Nombre, TipoDeDatoParametro, TipoValorParametro, ValorDelParametro)
                                 VALUES (@IDBeneficio, @Nombre, @TipoDeDatoParametro, @TipoValorParametro, @ValorDelParametro);";
                _conexion.Open();
                foreach (var p in parametros)
                {
                    using (var comandoParaConsulta = new SqlCommand(query, _conexion))
                    {
                        comandoParaConsulta.Parameters.AddWithValue("@IDBeneficio", idBeneficio);
                        comandoParaConsulta.Parameters.AddWithValue("@Nombre", p.Nombre);
                        comandoParaConsulta.Parameters.AddWithValue("@TipoDeDatoParametro", p.TipoDeDatoParametro);
                        comandoParaConsulta.Parameters.AddWithValue("@TipoValorParametro", p.TipoValorParametro);
                        comandoParaConsulta.Parameters.AddWithValue("@ValorDelParametro", p.ValorDelParametro);

                        comandoParaConsulta.ExecuteNonQuery();
                    }
                }
                exito = true;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                throw new Exception("Error al crear parametros de beneficio en la base de datos");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("Ocurrió un error inesperado en BeneficiosRepository.CrearParametros");
            }
            finally
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
            return exito;
        }

        public bool ExisteBeneficio(string cedulaEmpresa, string nombreBeneficio)
        {
            try
            {
                var consulta = @"SELECT * FROM Beneficio
                                 WHERE Beneficio.Nombre = @NombreBeneficio
                                 AND Beneficio.CedulaEmpresa = @CedulaEmpresa;";
                var comandoParaConsulta = new SqlCommand(consulta, _conexion);

                comandoParaConsulta.Parameters.AddWithValue("@NombreBeneficio", nombreBeneficio);
                comandoParaConsulta.Parameters.AddWithValue("@CedulaEmpresa", cedulaEmpresa);
                _conexion.Open();
                var reader = comandoParaConsulta.ExecuteReader();
                return reader.HasRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error buscando el beneficio: {ex.Message}");
                return false;
            }
            finally
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
        }

        public int CopiarAPI(string nombreBeneficio, string cedulaEmpresa, int idUsuario)
        {
            int idBeneficio = -1;
#pragma warning disable IDE0063
            try
            {
                var consulta = @"INSERT INTO [dbo].[Beneficio] ([Nombre], [Tipo] ,[Descripcion],
                                             [MesesMinimos], [CantidadParametros], [UsuarioCrea],
                                             [UsuarioModifica], [CedulaEmpresa])
                                 SELECT Nombre, Tipo, Descripcion, MesesMinimos, CantidadParametros,
                                        @UsuarioCrea, @UsuarioModifica, @NuevaCedulaEmpresa
                                 FROM Beneficio
                                 WHERE Nombre = @NombreBeneficio
                                 AND
                                 Tipo = 'API'
                                 AND CedulaEmpresa IS NULL
                                 SELECT SCOPE_IDENTITY() AS IDBeneficio;";
                using (var comandoParaConsulta = new SqlCommand(consulta, _conexion))
                {
                    comandoParaConsulta.Parameters.AddWithValue("@UsuarioCrea", idUsuario);
                    comandoParaConsulta.Parameters.AddWithValue("@UsuarioModifica", idUsuario);
                    comandoParaConsulta.Parameters.AddWithValue("@NuevaCedulaEmpresa", cedulaEmpresa);
                    comandoParaConsulta.Parameters.AddWithValue("@NombreBeneficio", nombreBeneficio);

                    _conexion.Open();

                    using (var reader = comandoParaConsulta.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idBeneficio = Convert.ToInt32(reader["IDBeneficio"]);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                throw new Exception("Error al copiar beneficio en la base de datos");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("Ocurrió un error inesperado en BeneficiosRepository.CopiarAPI");
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

        public bool CopiarParametros(int idBeneficioNuevo, string nombreBeneficioCopiar)
        {
            bool exito = false;
            try
            {
                var consulta = @"INSERT INTO [dbo].[API] ([IdBeneficio], [ServicioExterno],
                                             [Metodo], [NombreKey], [ValorKey])
                                 SELECT @IdNuevo, a.ServicioExterno, a.Metodo, a.NombreKey, a.ValorKey
                                 FROM API a
                                 JOIN Beneficio b ON b.ID = a.IdBeneficio
                                 WHERE b.Nombre = @nombreBeneficioCopiar
                                 AND b.CedulaEmpresa IS NULL;";
                using (var comandoParaConsulta = new SqlCommand(consulta, _conexion))
                {
                    comandoParaConsulta.Parameters.AddWithValue("@IdNuevo", idBeneficioNuevo);
                    comandoParaConsulta.Parameters.AddWithValue("@nombreBeneficioCopiar", nombreBeneficioCopiar);

                    _conexion.Open();

                    exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
                }

                if (exito)
                {
                    consulta = @"INSERT INTO [dbo].[ParametrosBeneficio] ([IDBeneficio],
                                             [Nombre], [TipoDeDatoParametro], [ValorDelParametro], [TipoValorParametro])
                                 SELECT @IdNuevo, pb.Nombre, pb.TipoDeDatoParametro, pb.ValorDelParametro, pb.TipoValorParametro
                                 FROM ParametrosBeneficio pb
                                 JOIN Beneficio b ON b.ID = pb.IDBeneficio
                                 WHERE b.Nombre = @nombreBeneficioCopiar
                                 AND b.CedulaEmpresa IS NULL;";
                    using (var comandoParaConsulta = new SqlCommand(consulta, _conexion))
                    {
                        comandoParaConsulta.Parameters.AddWithValue("@IdNuevo", idBeneficioNuevo);
                        comandoParaConsulta.Parameters.AddWithValue("@nombreBeneficioCopiar", nombreBeneficioCopiar);

                        exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                throw new Exception("Error al copiar parametros del beneficio en la base de datos");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception("Ocurrió un error inesperado en BeneficiosRepository.CopiarParametros");
            }
            finally
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
#pragma warning restore IDE0063
            return exito;
        }

        public bool SePuedeModificar(int idBeneficio, string cedulaEmpresa)
        {
            try
            {
                var consulta = @"SELECT * FROM EligeBeneficio
                                 WHERE EligeBeneficio.IDBeneficio = @IDBeneficio;";
                var comandoParaConsulta = new SqlCommand(consulta, _conexion);

                comandoParaConsulta.Parameters.AddWithValue("@IDBeneficio", idBeneficio);
                _conexion.Open();
                var reader = comandoParaConsulta.ExecuteReader();
                return !reader.HasRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en BeneficiosRepository.SePuedeModificar: {ex.Message}");
                throw;
            }
            finally
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
        }
        public bool ModificarBeneficio(BeneficioModel beneficioModificado, int idUsuario)
        {
            bool exito = false;
            try
            {
                var consulta = @"UPDATE Beneficio
                                 SET 
                                     Nombre = @NuevoNombre,
                                     Descripcion = @NuevaDescripcion,
                                     Tipo = @NuevoTipo,
                                     MesesMinimos = @NuevosMesesMinimos,
                                     CantidadParametros = @CantidadParametros,
                                     UsuarioModifica = @UsuarioModifica
                                 WHERE 
                                     Id = @IdBeneficio;";
                var comandoParaConsulta = new SqlCommand(consulta, _conexion);

                comandoParaConsulta.Parameters.AddWithValue("@IdBeneficio", beneficioModificado.Id);
                comandoParaConsulta.Parameters.AddWithValue("@NuevoNombre", beneficioModificado.Nombre);
                comandoParaConsulta.Parameters.AddWithValue("@NuevaDescripcion", beneficioModificado.Descripcion);
                comandoParaConsulta.Parameters.AddWithValue("@NuevoTipo", beneficioModificado.Tipo);
                comandoParaConsulta.Parameters.AddWithValue("@NuevosMesesMinimos", beneficioModificado.MesesMinimos);
                comandoParaConsulta.Parameters.AddWithValue("@CantidadParametros", beneficioModificado.CantidadParametros);
                comandoParaConsulta.Parameters.AddWithValue("@UsuarioModifica", idUsuario);
                _conexion.Open();
                exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en BeneficiosRepository.ModificarBeneficio: {ex.Message}");
                throw;
            }
            finally
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
            return exito;
        }

        public bool ModificarParametros(List<ParametroBeneficioModel> parametros, int idBeneficio)
        {
            bool exito = false;
            try
            {
                var consulta = @"DELETE FROM ParametrosBeneficio
                                 WHERE IDBeneficio = @IDBeneficio;";
                var comandoParaConsulta = new SqlCommand(consulta, _conexion);

                comandoParaConsulta.Parameters.AddWithValue("@IDBeneficio", idBeneficio);
                _conexion.Open();
                comandoParaConsulta.ExecuteNonQuery();
                _conexion.Close();
                exito = CrearParametros(parametros, idBeneficio);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en BeneficiosRepository.ModificarBeneficio: {ex.Message}");
                throw;
            }
            finally
            {
                if (_conexion.State == ConnectionState.Open)
                {
                    _conexion.Close();
                }
            }
            return exito;
        }
    }
}
