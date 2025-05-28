using backend_planilla.Domain;
using backend_planilla.Infraestructure;
using Microsoft.AspNetCore.Mvc;

namespace backend_planilla.Application
{
    public interface IBeneficiosQuery
    {
        List<BeneficioModel> GetBeneficios(string correo);
        bool CrearBeneficio(BeneficioModel beneficio, string correo);
    }
}
