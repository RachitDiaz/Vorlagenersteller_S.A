using backend_planilla.Domain;

namespace backend_planilla.Application
{
    public interface IGenerarPlanilla
    {
        Task<Guid> EjecutarAsync(GenerarPlanillaRequestModel request, ICalculoDeduccionesObligatorias calculadora, IGetDeduccionBeneficiosQuery beneficios);
    }
}
