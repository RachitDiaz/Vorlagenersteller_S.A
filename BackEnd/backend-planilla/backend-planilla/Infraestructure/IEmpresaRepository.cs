using backend_planilla.Domain;

namespace backend_planilla.Infraestructure
{
    public interface IEmpresaRepository
    {
        bool RegistrarEmpresa(AgregarEmpresaModel empresa);
    }
}
