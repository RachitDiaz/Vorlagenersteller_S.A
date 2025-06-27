using backend_planilla.Domain;

namespace backend_planilla.Infraestructure
{
    public interface IReportesRepository
    {
        public List<ReportePagoEmpleadoDTO> ObtenerUltimosPagosEmpleado(string cedulaEmpleado, int Cantidad);
        public List<ReportePagoEmpresaDTO> ObtenerUltimosPagosEmpresa(string cedulaEmpresa, int Cantidad);
    }
}
