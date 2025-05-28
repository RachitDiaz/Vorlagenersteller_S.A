using backend_planilla.Domain;
using backend_planilla.Application;
using backend_planilla.Infraestructure;

namespace backend_planilla.Application
{
    public class EmpresaQuery : IEmpresaQuery
    {
        private readonly IEmpresaRepository _empresaRepository;
        public EmpresaQuery() {
            _empresaRepository = new EmpresaRepository();
        }

        bool IEmpresaQuery.RegistrarEmpresa(AgregarEmpresaModel infoEmpresa, string correo)
        {
            // validate empresa input

            var resultado = _empresaRepository.RegistrarEmpresa(infoEmpresa, correo);
            return resultado;
        }
    }
}
