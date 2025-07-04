using System.Net.Mail;
using System.Net;
using backend_planilla.Domain;

namespace backend_planilla.Application
{
    public class NotificacionesEmail: INotificacionesEmail
    {
        private readonly string CORREO_VORLAGENERSTELLAR;
        private readonly string CONTRASENA_CORREO_VORLA;
        private readonly string CLIENTE_ENVIO_CORREO;
        private readonly int PUERTO;
        public NotificacionesEmail()
        {
            CORREO_VORLAGENERSTELLAR = "danieltestingpi@gmail.com";
            CONTRASENA_CORREO_VORLA = "iifr ejoo msry xqtx";
            CLIENTE_ENVIO_CORREO = "smtp.gmail.com";
            PUERTO = 587;
        }
        public bool enviarDocumentoPDF(SolicitudCorreoModel solicitud, IFormFile documentoPDF)
        {
            try
            {
                var mensaje = new MailMessage();
                mensaje.From = new MailAddress(CORREO_VORLAGENERSTELLAR);

                mensaje.To.Add(solicitud.destinatario);
                mensaje.Subject = solicitud.asunto;
                mensaje.Body = solicitud.mensaje;

                var smtpClient = new SmtpClient(CLIENTE_ENVIO_CORREO)
                {
                    Port = PUERTO,
                    Credentials = new NetworkCredential(CORREO_VORLAGENERSTELLAR, CONTRASENA_CORREO_VORLA),
                    EnableSsl = true,
                };


                using (var ms = new MemoryStream())
                {
                    documentoPDF.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    Attachment att = new Attachment(new MemoryStream(fileBytes), documentoPDF.FileName);
                    mensaje.Attachments.Add(att);
                }


                smtpClient.Send(mensaje);
            }
            catch
            {
                return false;
            }
      
            return true;
        }
    }
}
