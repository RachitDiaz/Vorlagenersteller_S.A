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


        public List<EmpresaModel> ObtenerEmpresas()
        {

            List<EmpresaModel> Empresa = new List<EmpresaModel>();
            string consulta = string.Concat("Select * from Empresa");

            DataTable tablaResultado = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                Empresa.Add(
                new EmpresaModel
                {
                    CedulaJuridica = Convert.ToString(columna["CedulaJuridica"]),
                    CedulaDueno = Convert.ToString(columna["CedulaDueno"]),
                    CedulaAdmin = Convert.ToString(columna["CedulaAdmin"]),
                    TipoDePago = Convert.ToString(columna["TipoDePago"]),
                    RazonSocial = Convert.ToString(columna["RazonSocial"]),
                    Nombre = Convert.ToString(columna["Nombre"]),
                    Descripcion = Convert.ToString(columna["Descripcion"]),
                    BeneficiosMaximos = Convert.ToInt32(columna["BeneficiosMaximos"]),
                    FechaDeCreacion = Convert.ToDateTime(columna["FechaDeCreacion"]),
                    FechaDeModificacion = Convert.ToDateTime(columna["FechaDeModificacion"]),
                    UsuarioCreador = Convert.ToInt32(columna["UsuarioCreador"]),
                    UltimoEnModificar = Convert.ToInt32(columna["UltimoEnModificar"]),
                    Activo = Convert.ToBoolean(columna["activo"])
                });
            }

            return Empresa;
        }

        public List<EmpresaModel> ObtenerEmpresa(string cedulaQuery)
        {
            List<EmpresaModel> Empresa = new List<EmpresaModel>();
            string consulta = string.Concat("Select * from Empresa where CedulaJuridica = '", cedulaQuery);
            consulta = string.Concat(consulta, "'");

            DataTable tablaResultado = CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                Empresa.Add(
                new EmpresaModel
                {
                    CedulaJuridica = Convert.ToString(columna["CedulaJuridica"]),
                    CedulaDueno = Convert.ToString(columna["CedulaDueno"]),
                    CedulaAdmin = Convert.ToString(columna["CedulaAdmin"]),
                    TipoDePago = Convert.ToString(columna["TipoDePago"]),
                    RazonSocial = Convert.ToString(columna["RazonSocial"]),
                    Nombre = Convert.ToString(columna["Nombre"]),
                    Descripcion = Convert.ToString(columna["Descripcion"]),
                    BeneficiosMaximos = Convert.ToInt32(columna["BeneficiosMaximos"]),
                    FechaDeCreacion = Convert.ToDateTime(columna["FechaDeCreacion"]),
                    FechaDeModificacion = Convert.ToDateTime(columna["FechaDeModificacion"]),
                    UsuarioCreador = Convert.ToInt32(columna["UsuarioCreador"]),
                    UltimoEnModificar = Convert.ToInt32(columna["UltimoEnModificar"]),
                    Activo = Convert.ToBoolean(columna["activo"])
                });
            }

            return Empresa;
        }

        public bool CrearEmpresa(EmpresaModel empresa)
        {
            var consulta = @"INSERT INTO Empresa (CedulaJuridica, CedulaDueno, CedulaAdmin, TipoDePago, RazonSocial, 
            Nombre, Descripcion, BeneficiosMaximos, FechaDeCreacion,FechaDeModificacion,
            UsuarioCreador, UltimoEnModificar, activo)
            VALUES(@CedulaJuridica, @CedulaDueno, @CedulaAdmin, @TipoDePago, @RazonSocial, 
            @Nombre, @Descripcion, @BeneficiosMaximos, @FechaDeCreacion, @FechaDeModificacion,
            @UsuarioCreador, @UltimoEnModificar, @activo) ";

            var comandoParaConsulta = new SqlCommand(consulta, _conexion);

            comandoParaConsulta.Parameters.AddWithValue("@CedulaJuridica", empresa.CedulaJuridica);
            comandoParaConsulta.Parameters.AddWithValue("@CedulaDueno", empresa.CedulaDueno);
            comandoParaConsulta.Parameters.AddWithValue("@CedulaAdmin", empresa.CedulaAdmin);
            comandoParaConsulta.Parameters.AddWithValue("@TipoDePago", empresa.TipoDePago);
            comandoParaConsulta.Parameters.AddWithValue("@RazonSocial", empresa.RazonSocial);
            comandoParaConsulta.Parameters.AddWithValue("@Nombre", empresa.Nombre);
            comandoParaConsulta.Parameters.AddWithValue("@Descripcion", empresa.Descripcion);
            comandoParaConsulta.Parameters.AddWithValue("@BeneficiosMaximos", empresa.BeneficiosMaximos);
            comandoParaConsulta.Parameters.AddWithValue("@FechaDeCreacion", empresa.FechaDeCreacion);
            comandoParaConsulta.Parameters.AddWithValue("@FechaDeModificacion", empresa.FechaDeModificacion);
            comandoParaConsulta.Parameters.AddWithValue("@UsuarioCreador", empresa.UsuarioCreador);
            comandoParaConsulta.Parameters.AddWithValue("@UltimoEnModificar", empresa.UltimoEnModificar);
            comandoParaConsulta.Parameters.AddWithValue("@activo", empresa.Activo);

            _conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            _conexion.Close();

            return exito;
        }

    }
}
