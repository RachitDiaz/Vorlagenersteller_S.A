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
        public List<BeneficioModel> ObtenerBeneficios()
        {
            List<BeneficioModel> beneficios = new List<BeneficioModel>();
            string consulta = "SELECT * FROM dbo.Beneficio ";
            DataTable tablaResultado =
            CrearTablaConsulta(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                beneficios.Add(
                new BeneficioModel
                {
                    Nombre = Convert.ToString(columna["Nombre"]),
                    Descripcion = Convert.ToString(columna["Descripcion"]),
                    Tipo = Convert.ToString(columna["Tipo"]),
                    ServicioExterno = Convert.ToString(columna["ServicioExterno"]),
                    MesesMinimos = Convert.ToInt32(columna["MesesMinimos"]),
                    CantidadParametros = Convert.ToInt32(columna["CantidadParametros"])
                });
            }
            return beneficios;
        }

        public bool CrearBeneficio(BeneficioModel beneficio)
        {
            var consulta = @"INSERT INTO [dbo].[Beneficio] ([Nombre], [Tipo] ,[Descripcion], [ServicioExterno], [MesesMinimos])
                                             VALUES(@Nombre, @Tipo , @Descripcion, @ServicioExterno, @MesesMinimos) ";
            var comandoParaConsulta = new SqlCommand(consulta, _conexion);

            comandoParaConsulta.Parameters.AddWithValue("@Nombre", beneficio.Nombre);
            comandoParaConsulta.Parameters.AddWithValue("@Tipo", beneficio.Tipo);
            comandoParaConsulta.Parameters.AddWithValue("@Descripcion", beneficio.Descripcion);
            comandoParaConsulta.Parameters.AddWithValue("@ServicioExterno", beneficio.ServicioExterno);
            comandoParaConsulta.Parameters.AddWithValue("@MesesMinimos", beneficio.MesesMinimos);

            _conexion.Open();
            bool exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            _conexion.Close();

            return exito;
        }

    }
}