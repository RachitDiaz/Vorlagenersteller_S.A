using backend_planilla.Models;
using backend_planilla.Domain;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using backend_planilla.Infraestructure;
using System.Data.Common;
using System.Security.Cryptography.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend_planilla.Infraestructure
{
    public class ReportesRepository: IReportesRepository
    {
        private SqlConnection _conexion;
        private string _rutaConexion;
        public ReportesRepository()
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            _rutaConexion = builder.Configuration.GetConnectionString("piTestContext");
            _conexion = new SqlConnection(_rutaConexion);
        }

        public ReportesRepository(SqlConnection _nuevaConexion, string _nuevaRuta)
        {
            _rutaConexion = _nuevaRuta;
            _conexion = _nuevaConexion;
        }

        public List<ReportePagoEmpleadoDTO> ObtenerUltimosPagosEmpleado(string cedulaEmpleado, int cantidad)
        {
            List<ReportePagoEmpleadoDTO> reportesEmpleado = new List<ReportePagoEmpleadoDTO>();
            string query = @"Select TOP (@Cantidad) Persona.Nombre, Persona.Apellido1, Persona.Apellido2,
				    Empleado.TipoContrato, Empresa.Nombre Empresa, P.FechaDeCreacion,
				    P.SalarioBruto, P.SEMEmpleado, P.IVEMEmpleado, P.BPPOEmpleado,
				    P.ImpuestoRenta, P.BeneficioMonto1, P.BeneficioNombre1, P.FechaDeCreacion Fecha,
				    P.BeneficioMonto2, P.BeneficioNombre3,P.BeneficioMonto3,
				    P.BeneficioNombre3, P.TotalDeduccionesEmpleado, P.TotalDeduccionesBeneficios
		            FROM Persona
		            INNER JOIN Empleado ON Empleado.CedulaEmpleado = Persona.Cedula
		            INNER JOIN Empresa ON Empresa.CedulaJuridica = Empleado.CedulaEmpresa
		            INNER JOIN PlanillaMensualEmpleado P ON P.CedulaEmpleado = Empleado.CedulaEmpleado
		            WHERE Persona.Cedula = @CedulaEmpleado
		            ORDER BY P.FechaDeCreacion DESC;";

            SqlCommand comandoParaConsulta = new SqlCommand(query, _conexion);
            comandoParaConsulta.Parameters.AddWithValue("@CedulaEmpleado", cedulaEmpleado);
            comandoParaConsulta.Parameters.AddWithValue("@Cantidad", cantidad);

            try
            {
                if (_conexion.State != ConnectionState.Open) { _conexion.Open(); }

                SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
                DataTable consultaFormatoTabla = new DataTable();
                adaptadorParaTabla.Fill(consultaFormatoTabla);

                _conexion.Close();

                foreach (DataRow columna in consultaFormatoTabla.Rows)
                {
                    decimal loopBeneficioCosto1 = 0;
                    decimal loopBeneficioCosto2 = 0;
                    decimal loopBeneficioCosto3 = 0;
                    string nombreCompleto = Convert.ToString(columna["Nombre"]) + " " +
                        Convert.ToString(columna["Apellido1"]) + " " + Convert.ToString(columna["Apellido2"]);

                    if (!columna["BeneficioMonto1"].ToString().Equals(""))
                    {
                        loopBeneficioCosto1 = Convert.ToDecimal(columna["BeneficioCosto1"]);
                    }

                    if (!columna["BeneficioMonto2"].ToString().Equals(""))
                    {
                        loopBeneficioCosto2 = Convert.ToDecimal(columna["BeneficioCosto2"]);
                    }

                    if (!columna["BeneficioMonto3"].ToString().Equals(""))
                    {
                        loopBeneficioCosto3 = Convert.ToDecimal(columna["BeneficioCosto3"]);
                    }

                    reportesEmpleado.Add(
                    new ReportePagoEmpleadoDTO
                    {
                        NombreEmpresa = Convert.ToString(columna["Empresa"]),
                        NombreEmpleado = nombreCompleto,
                        TipoContrato = Convert.ToString(columna["TipoContrato"]),
                        Fecha = Convert.ToString(columna["Fecha"]),
                        SalarioBruto = Convert.ToDecimal(columna["SalarioBruto"]),
                        SEM = Convert.ToDecimal(columna["SEMEmpleado"]),
                        IVM = Convert.ToDecimal(columna["IVEMEmpleado"]),
                        BPP = Convert.ToDecimal(columna["BPPOEmpleado"]),
                        Renta = Convert.ToDecimal(columna["ImpuestoRenta"]),
                        BeneficioNombre1 = columna["BeneficioMonto1"].ToString(),
                        BeneficioCosto1 = loopBeneficioCosto1,
                        BeneficioNombre2 = columna["BeneficioMonto2"].ToString(),
                        BeneficioCosto2 = loopBeneficioCosto2,
                        BeneficioNombre3 = columna["BeneficioMonto3"].ToString(),
                        BeneficioCosto3 = loopBeneficioCosto3,
                        TotalDeduccionesEmpleado = Convert.ToDecimal(columna["TotalDeduccionesEmpleado"]),
                        TotalDeduccionesBeneficios = Convert.ToDecimal(columna["TotalDeduccionesBeneficios"])
                    });
                }
            }
            finally
            {
                if (_conexion.State != ConnectionState.Closed) { _conexion.Close(); }
            }

            return reportesEmpleado;
        }

    }
}
