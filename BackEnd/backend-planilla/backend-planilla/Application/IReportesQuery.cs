using backend_planilla.Domain;
using backend_planilla.Infraestructure;
using backend_planilla.Models;

namespace backend_planilla.Application
{
    public interface IReportesQuery
    {
        public List<ReportePagoEmpleadoDTO>? ObtenerUltimosPagosEmpleado(string cedula);
        public List<ReportePagoEmpresaDTO>? ObtenerUltimosPagosEmpresa(string cedula);
        public List<ReportePagoEmpresaDTO>? ObtenerPagosHistoricosEmpresa(string cedula);
        public bool enviarEmailReporte(IFormFile documentoPDF, string correoDestinatario);
    }
}
