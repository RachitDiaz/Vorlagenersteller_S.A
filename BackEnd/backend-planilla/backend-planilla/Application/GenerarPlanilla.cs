using backend_planilla.Domain;
using backend_planilla.Infraestructure;

namespace backend_planilla.Application
{
    public class GenerarPlanilla
    {
        private readonly IPlanillaRepository _planillaRepository;
        private readonly GenerarCalculosQuery _calculosQuery;

        public GenerarPlanilla(IPlanillaRepository planillaRepository, GenerarCalculosQuery calculosQuery)
        {
            _planillaRepository = planillaRepository;
            _calculosQuery = calculosQuery;
        }

        public async Task<Guid> EjecutarAsync(GenerarPlanillaRequestModel request, ICalculoDeduccionesObligatorias calculadora, IGetDeduccionBeneficiosQuery beneficios)
        {

            var resultados = await _calculosQuery.ObtenerResultadosAsync(request.CedulaJuridica, request.TipoPlanilla, calculadora, beneficios);
            var idPlanilla = await _planillaRepository.InsertarPlanillaCompletaAsync(request.CedulaJuridica, request.Periodo, request.FechaGeneracion, resultados);

            return idPlanilla;
        }
    }
}
