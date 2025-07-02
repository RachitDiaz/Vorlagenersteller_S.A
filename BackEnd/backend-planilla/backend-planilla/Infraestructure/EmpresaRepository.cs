using backend_planilla.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace backend_planilla.Infraestructure
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private SqlConnection _conexion;
        private string _rutaConexion;

        public EmpresaRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _rutaConexion =
            builder.Configuration.GetConnectionString("piTestContext");
            _conexion = new SqlConnection(_rutaConexion);
        }
        bool IEmpresaRepository.RegistrarEmpresa(AgregarEmpresaModel infoEmpresa, string correo)
        {
            var consulta = @"EXECUTE RegistrarEmpresa
	                    @CedulaJuridica = @@CedulaJuridica,
	                    @CedulaDueno = @@CedulaDueno,
	                    @TipoDePago = @@TipoDePago,
                        @RazonSocial = @@RazonSocial,
	                    @Nombre = @@Nombre,
	                    @Descripcion = @@Descripcion,
	                    @BeneficiosMaximos = @@BeneficiosMaximos,
	                    @CorreoCreador = @@CorreoDelCreador,
	                    @CorreoEmpresa = @@CorreoEmpresa,
	                    @Telefono = @@Telefono,
	                    @Provincia = @@Provincia,
	                    @Canton = @@Canton,
	                    @Distrito = @@Distrito,
	                    @OtrasSenas = @@OtrasSenas";

            var comandoParaConsulta = new SqlCommand(consulta, _conexion);

            comandoParaConsulta.Parameters.AddWithValue("@@CedulaJuridica", infoEmpresa.CedulaJuridica);
            comandoParaConsulta.Parameters.AddWithValue("@@CedulaDueno", infoEmpresa.CedulaDueno);
            comandoParaConsulta.Parameters.AddWithValue("@@CedulaAdmin", "1-1909-0924");
            comandoParaConsulta.Parameters.AddWithValue("@@TipoDePago", infoEmpresa.TipoDePago);
            comandoParaConsulta.Parameters.AddWithValue("@@RazonSocial", infoEmpresa.RazonSocial);
            comandoParaConsulta.Parameters.AddWithValue("@@Nombre", infoEmpresa.Nombre);
            comandoParaConsulta.Parameters.AddWithValue("@@Descripcion", infoEmpresa.Descripcion);
            comandoParaConsulta.Parameters.AddWithValue("@@BeneficiosMaximos", infoEmpresa.BeneficiosMaximos);
            comandoParaConsulta.Parameters.AddWithValue("@@CorreoDelCreador", correo);
            comandoParaConsulta.Parameters.AddWithValue("@@CorreoEmpresa", infoEmpresa.Correo);
            comandoParaConsulta.Parameters.AddWithValue("@@Telefono", infoEmpresa.Telefono);
            comandoParaConsulta.Parameters.AddWithValue("@@Provincia", infoEmpresa.Provincia);
            comandoParaConsulta.Parameters.AddWithValue("@@Canton", infoEmpresa.Canton);
            comandoParaConsulta.Parameters.AddWithValue("@@Distrito", infoEmpresa.Distrito);
            comandoParaConsulta.Parameters.AddWithValue("@@OtrasSenas", infoEmpresa.OtrasSenas);

            _conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            _conexion.Close();

            return exito;
        }
        public List<string> EliminarEmpresa(string cedulaEmpresa)
        {
            List<string> listaCorreos = new();
            _conexion.Open();
            var transaccion = _conexion.BeginTransaction();
            try
            {
                Console.WriteLine(cedulaEmpresa + " Eliminar");
                var consulta = @"
                                SELECT Correo FROM Usuario u
                                INNER JOIN Persona p ON u.Cedula = p.Cedula
                                INNER JOIN Empleado e ON p.Cedula = e.CedulaEmpleado
                                WHERE CedulaEmpresa = @cedulaEmpresa;";
                var comandoParaConsulta = new SqlCommand(consulta, _conexion, transaccion);
                Console.WriteLine("Read start");

                comandoParaConsulta.Parameters.AddWithValue("@cedulaEmpresa", cedulaEmpresa);
                var reader = comandoParaConsulta.ExecuteReader();
                while (reader.Read())
                {
                    string correo = Convert.ToString(reader["Correo"]);
                    listaCorreos.Add(correo);
                    Console.WriteLine(correo);
                }
                reader.Close();

                Console.WriteLine("Delete start");
                consulta = @"DELETE FROM Empresa WHERE CedulaJuridica = @cedulaEmpresa;";
                comandoParaConsulta = new SqlCommand(consulta, _conexion, transaccion);
                comandoParaConsulta.Parameters.AddWithValue("@cedulaEmpresa", cedulaEmpresa);

                if (comandoParaConsulta.ExecuteNonQuery() >= 1)
                {
                    transaccion.Commit();
                    return listaCorreos;
                }
                else
                {
                    throw new Exception("Ocurrió un error al eliminar la empresa2");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "  a");
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
                    _conexion.Close();
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
                        _conexion.Close();
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
    }
}
