using backend_planilla.Domain;
using backend_planilla.Handlers;
using backend_planilla.Infraestructure;
using backend_planilla.Models;
using System.Net;
using System.Net.Mail;

namespace backend_planilla.Application
{
    public class EmpleadoQuery : IEmpleadoQuery
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly string CORREO_VORLAGENERSTELLAR = "danieltestingpi@gmail.com";
        private readonly string CONTRASENA_CORREO_VORLA = "iifr ejoo msry xqtx";
        private readonly string ASUNTO_CORREO_ELIMINACION = "Notificación de eliminación de cuenta";
        private readonly string MENSAJE_CORREO_ELIMINACION = "Su cuenta ha sido eliminada del sistema";
        private readonly string CLIENTE_ENVIO_CORREO = "smtp.gmail.com";
        public EmpleadoQuery() {
            _empleadoRepository = new EmpleadoRepository();
        }
        public EmpleadoQuery(IEmpleadoRepository empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }
        public bool CrearEmpleado(PersonaModel persona, EmpleadoModel empleado, string correo)
        {
            return _empleadoRepository.CrearEmpleado(persona, empleado, correo);
        }
        public bool EditarInfoEmpleado(InfoEmpleadoModel datosNuevos, string cedulaEmpleado)
        {
            if (datosNuevos == null) return false;
            InfoEmpleadoModel original;

            try
            {
                original = ObtenerInfoEmpleado(cedulaEmpleado);
                if (original == null) return false;
            }
            catch (Exception error)
            {
                throw;
            }

            if (!PermiteEdicion(original, datosNuevos)) return false;

            return _empleadoRepository.EditarInfoEmpleado(datosNuevos, cedulaEmpleado);
        }
        public InfoEmpleadoModel? ObtenerInfoEmpleado(string cedulaEmpleado)
        {
            return _empleadoRepository.ObtenerInfoEmpleado(cedulaEmpleado);
        }

        public InfoEmpleadoModel? ObtenerInfoEmpleadoCorreo(string correo)
        {
            string cedulaEmpleado = _empleadoRepository.ObtenerCedulaEmpleado(correo);
            return _empleadoRepository.ObtenerInfoEmpleado(cedulaEmpleado);
        }
        public List<EmpleadoModel> ObtenerEmpleados(string correo)
        {
            return _empleadoRepository.ObtenerEmpleados(correo);
        }

        public string ObtenerCedulaEmpleado(string correo)
        {
            return _empleadoRepository.ObtenerCedulaEmpleado(correo);
        }

        private static bool PermiteEdicion(InfoEmpleadoModel original, InfoEmpleadoModel nuevo)
        {
            if (original == null) return false;
            if (nuevo.CedulaEditable != original.CedulaEditable) return false;
            if (original.CedulaEditable) return true;

            bool valido = true;
            if (!original.Empleado.Nombre.Equals(nuevo.Empleado.Nombre)) valido = false;
            if (!original.Empleado.Apellido1.Equals(nuevo.Empleado.Apellido1)) valido = false;
            if (!original.Empleado.Apellido2.Equals(nuevo.Empleado.Apellido2)) valido = false;
            if (!original.Empleado.CedulaEmpleado.Equals(nuevo.Empleado.CedulaEmpleado)) valido = false;

            return valido;
        }

        public bool EliminarEmpleado(string cedulaEmpleado)
        {
            try
            {
                string correoEmpleado = _empleadoRepository.EliminarEmpleado(cedulaEmpleado);
                if (string.IsNullOrEmpty(correoEmpleado)) {
                    return false;
                }
                EnviarCorreoEmpleado(correoEmpleado, ASUNTO_CORREO_ELIMINACION, MENSAJE_CORREO_ELIMINACION);

                return true;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex) 
            {
                throw new Exception("Error al eliminar el empleado", ex);
            }

        }

        private void EnviarCorreoEmpleado(string destinatarioCorreo, string asuntoCorreo, string mensajeCorreo)
        {
            try
            {
                var mensaje = new MailMessage();
                mensaje.From = new MailAddress(CORREO_VORLAGENERSTELLAR);
                mensaje.To.Add(destinatarioCorreo);
                mensaje.Subject = asuntoCorreo;
                mensaje.Body = mensajeCorreo;
                var smtpClient = new SmtpClient(CLIENTE_ENVIO_CORREO)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(CORREO_VORLAGENERSTELLAR, CONTRASENA_CORREO_VORLA),
                    EnableSsl = true,
                };

                smtpClient.Send(mensaje);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
