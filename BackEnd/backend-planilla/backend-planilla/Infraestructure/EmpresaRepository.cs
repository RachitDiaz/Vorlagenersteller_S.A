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
    }
}
