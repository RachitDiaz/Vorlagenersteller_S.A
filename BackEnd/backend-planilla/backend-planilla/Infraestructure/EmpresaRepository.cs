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
        bool IEmpresaRepository.RegistrarEmpresa(AgregarEmpresaModel infoEmpresa)
        {
            var consulta = @"INSERT INTO Empresa (CedulaJuridica, CedulaDueno, CedulaAdmin, TipoDePago, RazonSocial, 
            Nombre, Descripcion, BeneficiosMaximos, FechaDeCreacion,FechaDeModificacion,
            UsuarioCreador, UltimoEnModificar, activo)
            VALUES(@CedulaJuridica, @CedulaDueno, @CedulaAdmin, @TipoDePago, @RazonSocial, 
            @Nombre, @Descripcion, @BeneficiosMaximos, @FechaDeCreacion, @FechaDeModificacion,
            @UsuarioCreador, @UltimoEnModificar, @activo) ";

            var comandoParaConsulta = new SqlCommand(consulta, _conexion);

            comandoParaConsulta.Parameters.AddWithValue("@CedulaJuridica", infoEmpresa.empresa.CedulaJuridica);
            comandoParaConsulta.Parameters.AddWithValue("@CedulaDueno", infoEmpresa.empresa.CedulaDueno);
            comandoParaConsulta.Parameters.AddWithValue("@CedulaAdmin", infoEmpresa.empresa.CedulaAdmin);
            comandoParaConsulta.Parameters.AddWithValue("@TipoDePago", infoEmpresa.empresa.TipoDePago);
            comandoParaConsulta.Parameters.AddWithValue("@RazonSocial", infoEmpresa.empresa.RazonSocial);
            comandoParaConsulta.Parameters.AddWithValue("@Nombre", infoEmpresa.empresa.Nombre);
            comandoParaConsulta.Parameters.AddWithValue("@Descripcion", infoEmpresa.empresa.Descripcion);
            comandoParaConsulta.Parameters.AddWithValue("@BeneficiosMaximos", infoEmpresa.empresa.BeneficiosMaximos);
            comandoParaConsulta.Parameters.AddWithValue("@FechaDeCreacion", infoEmpresa.empresa.FechaDeCreacion);
            comandoParaConsulta.Parameters.AddWithValue("@FechaDeModificacion", infoEmpresa.empresa.FechaDeModificacion);
            comandoParaConsulta.Parameters.AddWithValue("@UsuarioCreador", infoEmpresa.empresa.UsuarioCreador);
            comandoParaConsulta.Parameters.AddWithValue("@UltimoEnModificar", infoEmpresa.empresa.UltimoEnModificar);
            comandoParaConsulta.Parameters.AddWithValue("@activo", infoEmpresa.empresa.Activo);

            _conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            _conexion.Close();

            return exito;
        }
    }
}
