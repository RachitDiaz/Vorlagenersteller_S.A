using backend_planilla.Domain;
using backend_planilla.Infraestructure;
using Microsoft.AspNetCore.Mvc;

namespace backend_planilla.Application
{
    public interface IBeneficiosQuery
    {
        List<BeneficioModel> GetBeneficios(string correo);
        bool CrearBeneficio(BeneficioModel beneficio, string correo);
        bool CopiarBeneficioAPI(string nombreBeneficio, string cedulaEmpresa, int idUsuario);
        bool ModificarBeneficio(BeneficioModel beneficioModificado,
            BeneficioModel beneficioOriginal, string correo);
    }
}
