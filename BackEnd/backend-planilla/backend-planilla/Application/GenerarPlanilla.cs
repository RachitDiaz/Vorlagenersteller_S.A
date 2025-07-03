using backend_planilla.Domain;
using backend_planilla.Infraestructure;

namespace backend_planilla.Application
{
    public class GenerarPlanilla : IGenerarPlanilla
    {
        private readonly IPlanillaRepository _planillaRepository;
        private readonly IGenerarCalculosQuery _calculosQuery;

        public GenerarPlanilla(IPlanillaRepository planillaRepository, IGenerarCalculosQuery calculosQuery)
        {
            _planillaRepository = planillaRepository;
            _calculosQuery = calculosQuery;
        }

        public async Task<Guid> EjecutarAsync(GenerarPlanillaRequestModel request, ICalculoDeduccionesObligatorias calculadora, IGetDeduccionBeneficiosQuery beneficios)
        {
            string tipoPlanilla = await _planillaRepository.GetTipoDePagoAsync(request.CedulaJuridica);
            string periodo = GenerarPeriodo(tipoPlanilla);
            
            bool yaExiste = await _planillaRepository.ExistePeriodoAsync(request.CedulaJuridica, periodo);
            /*if (yaExiste)
            {
                throw new InvalidOperationException($"Ya existe una planilla generada para el período '{periodo}'.");
            }*/
            DateTime fechaGeneracion = DateTime.Today;
            var resultados = await _calculosQuery.ObtenerResultadosAsync(request.CedulaJuridica, tipoPlanilla, calculadora, beneficios);
            var idPlanilla = await _planillaRepository.InsertarPlanillaCompletaAsync(request.CedulaJuridica, periodo, fechaGeneracion, resultados, tipoPlanilla);

            return idPlanilla;
        }

        public static string GenerarPeriodo(string tipoPlanilla)
        {
            var fecha = DateTime.Today;
            var mes = fecha.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
            mes = char.ToUpper(mes[0]) + mes.Substring(1);
            var anio = fecha.Year;

            if (tipoPlanilla.ToLower() == "mensual")
            {
                return $"{mes} {anio}";
            }
            else if (tipoPlanilla.ToLower() == "quincenal")
            {
                int dia = fecha.Day;

                if (dia <= 15)
                    return $"15 {mes} {anio}";
                else
                {
                    int ultimoDiaMes = DateTime.DaysInMonth(anio, fecha.Month);
                    return $"{ultimoDiaMes} {mes} {anio}";
                }
            }

            return string.Empty;
        }
    }
}
