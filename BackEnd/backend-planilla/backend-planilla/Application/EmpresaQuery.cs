using backend_planilla.Domain;
using backend_planilla.Application;
using backend_planilla.Infraestructure;
using backend_planilla.Handlers;
using System.Net.Mail;
using System.Net;

namespace backend_planilla.Application
{
    public class EmpresaQuery : IEmpresaQuery
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly string CORREO_VORLAGENERSTELLAR = "danieltestingpi@gmail.com";
        private readonly string CONTRASENA_CORREO_VORLA = "iifr ejoo msry xqtx";
        private readonly string ASUNTO_CORREO_ELIMINACION = "Notificación de eliminación de cuenta";
        private readonly string MENSAJE_CORREO_ELIMINACION = "Su cuenta ha sido eliminada del sistema";
        private readonly string CLIENTE_ENVIO_CORREO = "smtp.gmail.com";
        public EmpresaQuery() {
            _empresaRepository = new EmpresaRepository();
        }

        public EmpresaQuery(IEmpresaRepository empresaRepository)
        {
            _empresaRepository = empresaRepository;
        }

        bool IEmpresaQuery.RegistrarEmpresa(AgregarEmpresaModel infoEmpresa, string correo)
        {
            var tamanoDeCedula = 12;
            string[] opcionesDePago = { "Semanal", "Quincenal", "Mensual"};
            var tamanoDeRazon = 100;
            var tamanoDeNombre = 100;
            var tamanoDeDescripcion = 300;
            var tamanoDeCorreos = 300;
            var tamanoDeTelefono = 15;
            var tamanoDeDirecciones = 20;
            var tamanoDeOtrasSenas = 300;

            if(infoEmpresa.CedulaDueno.Length > tamanoDeCedula) return false;
            if (infoEmpresa.CedulaJuridica.Length > tamanoDeCedula) return false;
            if (!EstaEn(opcionesDePago, infoEmpresa.TipoDePago)) return false;
            if (infoEmpresa.RazonSocial.Length > tamanoDeRazon) return false;
            if (infoEmpresa.Nombre.Length > tamanoDeNombre) return false;
            if (infoEmpresa.Descripcion.Length > tamanoDeDescripcion) return false;
            if (infoEmpresa.Correo.Length > tamanoDeCorreos) return false;
            if (correo.Length > tamanoDeCorreos) return false;
            if (infoEmpresa.Telefono.Length > tamanoDeTelefono) return false;
            if (infoEmpresa.Provincia.Length > tamanoDeDirecciones) return false;
            if (infoEmpresa.Canton.Length > tamanoDeDirecciones) return false;
            if (infoEmpresa.Distrito.Length > tamanoDeDirecciones) return false;
            if (infoEmpresa.OtrasSenas.Length > tamanoDeOtrasSenas) return false;

            var resultado = _empresaRepository.RegistrarEmpresa(infoEmpresa, correo);
            return resultado;
        }

        private bool EstaEn(string[] lista, string entrada)
        {
            foreach (string i in lista)
            {
                if (Object.Equals(i, entrada)) return true;
            }

            return false;
        }

        public bool EliminarEmpresa(string correo)
        {
            try
            {
                var cedulaEmpresa = _empresaRepository.ObtenerCedulaJuridica(correo);
                List<string> listaCorreos = _empresaRepository.EliminarEmpresa(cedulaEmpresa);
                for (int i = 0; i < listaCorreos.Count; i++)
                {
                    EnviarCorreoEmpleado(listaCorreos[i], ASUNTO_CORREO_ELIMINACION, MENSAJE_CORREO_ELIMINACION);
                }
                EnviarCorreoEmpleado(correo, ASUNTO_CORREO_ELIMINACION, MENSAJE_CORREO_ELIMINACION);

                return true;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la empresa", ex);
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
