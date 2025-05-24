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
            // validate empresa input

            var resultado = _empresaRepository.RegistrarEmpresa(infoEmpresa);
            return resultado;
        }
    }
}
