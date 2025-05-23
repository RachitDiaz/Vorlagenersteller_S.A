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

            comandoParaConsulta.Parameters.AddWithValue("@CedulaJuridica", infoEmpresa.CedulaJuridica);
            comandoParaConsulta.Parameters.AddWithValue("@CedulaDueno", infoEmpresa.CedulaDueno);
            comandoParaConsulta.Parameters.AddWithValue("@CedulaAdmin", "1-1907-0218");
            comandoParaConsulta.Parameters.AddWithValue("@TipoDePago", infoEmpresa.TipoDePago);
            comandoParaConsulta.Parameters.AddWithValue("@RazonSocial", infoEmpresa.RazonSocial);
            comandoParaConsulta.Parameters.AddWithValue("@Nombre", infoEmpresa.Nombre);
            comandoParaConsulta.Parameters.AddWithValue("@Descripcion", infoEmpresa.Descripcion);
            comandoParaConsulta.Parameters.AddWithValue("@BeneficiosMaximos", infoEmpresa.BeneficiosMaximos);
            comandoParaConsulta.Parameters.AddWithValue("@FechaDeCreacion", DateTime.Today);
            comandoParaConsulta.Parameters.AddWithValue("@FechaDeModificacion", DateTime.Today);
            comandoParaConsulta.Parameters.AddWithValue("@UsuarioCreador", "1");
            comandoParaConsulta.Parameters.AddWithValue("@UltimoEnModificar", "1");
            comandoParaConsulta.Parameters.AddWithValue("@activo", "1");

            _conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            _conexion.Close();

            return exito;
        }
    }
}
