using System.Collections.Generic;
using backend_planilla.Domain;
using backend_planilla.Infraestructure;
using backend_planilla.Models;
using Microsoft.Data.SqlClient;

namespace backend_planilla.Application
{
    public class ReportesQuery: IReportesQuery
    {
        IReportesRepository _reportesRepository;
        public ReportesQuery()
        {
            _reportesRepository = new ReportesRepository();
        }

        public ReportesQuery(ReportesRepository _nuevoReportesRepository)
        {
            _reportesRepository = _nuevoReportesRepository;
        }
        public List<ReportePagoEmpleadoDTO> ObtenerUltimosPagosEmpleado(string cedulaEmpleado)
        {
            int cantidadARecuperar = 10;

            List<ReportePagoEmpleadoDTO> resultado = _reportesRepository.ObtenerUltimosPagosEmpleado(cedulaEmpleado, cantidadARecuperar);
            return resultado;
        }

        public List<ReportePagoEmpresaDTO> ObtenerUltimosPagosEmpresa(string cedulaDueno)
        {
            int cantidadARecuperar = 10;

            List<ReportePagoEmpresaDTO> resultado = _reportesRepository.ObtenerUltimosPagosEmpresa(cedulaDueno, cantidadARecuperar);
            return resultado;
        }
    }
}
