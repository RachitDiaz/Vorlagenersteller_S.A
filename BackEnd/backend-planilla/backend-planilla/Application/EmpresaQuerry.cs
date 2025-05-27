using backend_planilla.Domain;
using backend_planilla.Application;
using backend_planilla.Infraestructure;

namespace backend_planilla.Application
{
    public class EmpresaQuerry : IEmpresaQuerry
    {
        private readonly IEmpresaRepository _empresaRepository;
        public EmpresaQuerry() {
            _empresaRepository = new EmpresaRepository();
        }

        bool IEmpresaQuerry.RegistrarEmpresa(AgregarEmpresaModel infoEmpresa)
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
            if (infoEmpresa.CorreoDelCreador.Length > tamanoDeCorreos) return false;
            if (infoEmpresa.Telefono.Length > tamanoDeTelefono) return false;
            if (infoEmpresa.Provincia.Length > tamanoDeDirecciones) return false;
            if (infoEmpresa.Canton.Length > tamanoDeDirecciones) return false;
            if (infoEmpresa.Distrito.Length > tamanoDeDirecciones) return false;
            if (infoEmpresa.OtrasSenas.Length > tamanoDeOtrasSenas) return false;

            var resultado = _empresaRepository.RegistrarEmpresa(infoEmpresa);
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
    }
}
