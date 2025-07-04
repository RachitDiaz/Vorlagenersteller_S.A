using backend_planilla.Domain;

namespace backend_planilla.Application
{
    public interface INotificacionesEmail
    {
        public bool enviarDocumentoPDF(SolicitudCorreoModel solicitud, IFormFile documentoPDF);
    }
}
