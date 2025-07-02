using backend_planilla.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace backend_planilla.Handlers
{
    public class EmpresaHandler
    {
        private SqlConnection _conexion;
        private string _rutaConexion;

        public EmpresaHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _rutaConexion = builder.Configuration.GetConnectionString("piTestContext");
            _conexion = new SqlConnection(_rutaConexion);
        }

        public List<EmpresaModel> ObtenerEmpresas(string correo)
        {
            List<EmpresaModel> empresas = new List<EmpresaModel>();

            var consulta = @"SELECT *
                             FROM Usuario u
                             JOIN Empresa e ON u.Cedula = e.CedulaAdmin OR u.Cedula = e.CedulaDueno
                             WHERE u.Correo = @CorreoUsuario;";

            var comando = new SqlCommand(consulta, _conexion);
            comando.Parameters.AddWithValue("@CorreoUsuario", correo);

            _conexion.Open();
            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    empresas.Add(new EmpresaModel
                    {
                        CedulaJuridica = reader["CedulaJuridica"].ToString(),
                        CedulaDueno = reader["CedulaDueno"].ToString(),
                        CedulaAdmin = reader["CedulaAdmin"].ToString(),
                        TipoDePago = reader["TipoDePago"].ToString(),
                        RazonSocial = reader["RazonSocial"].ToString(),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        BeneficiosMaximos = Convert.ToInt32(reader["BeneficiosMaximos"]),
                        FechaDeCreacion = Convert.ToDateTime(reader["FechaDeCreacion"]),
                        FechaDeModificacion = Convert.ToDateTime(reader["FechaDeModificacion"]),
                        UsuarioCreador = Convert.ToInt32(reader["UsuarioCreador"]),
                        UltimoEnModificar = Convert.ToInt32(reader["UltimoEnModificar"]),
                        Activo = Convert.ToBoolean(reader["activo"])
                    });
                }
            }
            _conexion.Close();

            return empresas;
        }

        public List<EmpresaModel> ObtenerEmpresa(string cedulaQuery)
        {
            List<EmpresaModel> empresas = new List<EmpresaModel>();

            var consulta = "SELECT * FROM Empresa WHERE CedulaJuridica = @Cedula";
            var comando = new SqlCommand(consulta, _conexion);
            comando.Parameters.AddWithValue("@Cedula", cedulaQuery);

            _conexion.Open();
            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    empresas.Add(new EmpresaModel
                    {
                        CedulaJuridica = reader["CedulaJuridica"].ToString(),
                        CedulaDueno = reader["CedulaDueno"].ToString(),
                        CedulaAdmin = reader["CedulaAdmin"].ToString(),
                        TipoDePago = reader["TipoDePago"].ToString(),
                        RazonSocial = reader["RazonSocial"].ToString(),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        BeneficiosMaximos = Convert.ToInt32(reader["BeneficiosMaximos"]),
                        FechaDeCreacion = Convert.ToDateTime(reader["FechaDeCreacion"]),
                        FechaDeModificacion = Convert.ToDateTime(reader["FechaDeModificacion"]),
                        UsuarioCreador = Convert.ToInt32(reader["UsuarioCreador"]),
                        UltimoEnModificar = Convert.ToInt32(reader["UltimoEnModificar"]),
                        Activo = Convert.ToBoolean(reader["activo"])
                    });
                }
            }
            _conexion.Close();

            return empresas;
        }

        public bool CrearEmpresa(EmpresaModel empresa, string correo)
        {
            var consulta = @"INSERT INTO Empresa (
                                CedulaJuridica, CedulaDueno, CedulaAdmin, TipoDePago, RazonSocial, 
                                Nombre, Descripcion, BeneficiosMaximos, FechaDeCreacion, FechaDeModificacion,
                                UsuarioCreador, UltimoEnModificar, activo)
                             VALUES (
                                @CedulaJuridica, @CedulaDueno, @CedulaAdmin, @TipoDePago, @RazonSocial, 
                                @Nombre, @Descripcion, @BeneficiosMaximos, @FechaDeCreacion, @FechaDeModificacion,
                                @UsuarioCreador, @UltimoEnModificar, @activo)";

            var comando = new SqlCommand(consulta, _conexion);
            comando.Parameters.AddWithValue("@CedulaJuridica", empresa.CedulaJuridica);
            comando.Parameters.AddWithValue("@CedulaDueno", empresa.CedulaDueno);
            comando.Parameters.AddWithValue("@CedulaAdmin", empresa.CedulaAdmin);
            comando.Parameters.AddWithValue("@TipoDePago", empresa.TipoDePago);
            comando.Parameters.AddWithValue("@RazonSocial", empresa.RazonSocial);
            comando.Parameters.AddWithValue("@Nombre", empresa.Nombre);
            comando.Parameters.AddWithValue("@Descripcion", empresa.Descripcion);
            comando.Parameters.AddWithValue("@BeneficiosMaximos", empresa.BeneficiosMaximos);
            comando.Parameters.AddWithValue("@FechaDeCreacion", empresa.FechaDeCreacion);
            comando.Parameters.AddWithValue("@FechaDeModificacion", empresa.FechaDeModificacion);
            comando.Parameters.AddWithValue("@UsuarioCreador", correo);
            comando.Parameters.AddWithValue("@UltimoEnModificar", empresa.UltimoEnModificar);
            comando.Parameters.AddWithValue("@activo", empresa.Activo);

            _conexion.Open();
            bool exito = comando.ExecuteNonQuery() >= 1;
            _conexion.Close();

            return exito;
        }
    }
}
