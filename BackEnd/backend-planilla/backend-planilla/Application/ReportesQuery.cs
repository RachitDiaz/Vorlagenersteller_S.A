using System.Text.RegularExpressions;
using backend_planilla.Domain;
using backend_planilla.Infraestructure;


namespace backend_planilla.Application
{
    public class ReportesQuery: IReportesQuery
    {
        IReportesRepository _reportesRepository;
        INotificacionesEmail _notificacionesEmail;
        public ReportesQuery()
        {
            _reportesRepository = new ReportesRepository();
            _notificacionesEmail = new NotificacionesEmail();
        }

        public ReportesQuery(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        public ReportesQuery(ReportesRepository _nuevoReportesRepository)
        {
            _reportesRepository = _nuevoReportesRepository;
        }
        public List<ReportePagoEmpleadoDTO>? ObtenerUltimosPagosEmpleado(string cedulaEmpleado)
        {
            if (!CedulaValida(cedulaEmpleado)) { return null; }

            int cantidadARecuperar = 10;

            List<ReportePagoEmpleadoDTO> resultado = _reportesRepository.ObtenerUltimosPagosEmpleado(cedulaEmpleado, cantidadARecuperar);
            return resultado;
        }

        public List<ReportePagoEmpresaDTO>? ObtenerUltimosPagosEmpresa(string cedulaDueno)
        {
            if (!CedulaValida(cedulaDueno)){ return null; }
            
            int cantidadARecuperar = 10;

            List<ReportePagoEmpresaDTO> resultado = _reportesRepository.ObtenerUltimosPagosEmpresa(cedulaDueno, cantidadARecuperar);
            return resultado;
        }

        private bool CedulaValida(string cedula)
        {
            string expresion = "\\d-\\d\\d\\d\\d-\\d\\d\\d\\d";
            Regex regex = new Regex(expresion);
            return regex.IsMatch(cedula);
        }

        public bool enviarEmailReporte(IFormFile documentoPDF, string correoDestinatario)
        {
            if (documentoPDF == null) return false;
            bool resultado = true;

            try
            {
                string _MensajeReporte = "Adjunto se encuentra el reporte solicitado\n\n" +
                    "Este es un mensaje automatizado, no responder a este correo.";
                string _AsuntoReporte = "Reporte";

                SolicitudCorreoModel solicitud = new SolicitudCorreoModel()
                {
                    destinatario = correoDestinatario,
                    asunto = _AsuntoReporte,
                    mensaje = _MensajeReporte
                };

                resultado = _notificacionesEmail.enviarDocumentoPDF(solicitud, documentoPDF);

            }
            catch (Exception excepcion) {
                throw new Exception();
            }

            return resultado;
        }
    }
}
