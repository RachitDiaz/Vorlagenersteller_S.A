using backend_planilla.Domain;

namespace backend_planilla.Application
{
    public interface IGenerarCalculosQuery
    {
        Task<List<ResultadoEmpleadoModel>> ObtenerResultadosAsync(
            string cedulaJuridica,
            string tipoPlanilla,
            ICalculoDeduccionesObligatorias calculadora,
            IGetDeduccionBeneficiosQuery beneficios);
    }
}
