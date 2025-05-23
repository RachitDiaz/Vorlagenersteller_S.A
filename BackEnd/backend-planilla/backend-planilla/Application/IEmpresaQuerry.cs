using Microsoft.AspNetCore.Mvc.RazorPages;
using backend_planilla.Domain;

namespace backend_planilla.Application
{
    public interface IEmpresaQuerry
    {
        bool RegistrarEmpresa(AgregarEmpresaModel empresa);
    }
}
