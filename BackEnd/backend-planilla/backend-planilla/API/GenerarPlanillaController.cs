using backend_planilla.Application;
using backend_planilla.Domain;
using Microsoft.AspNetCore.Mvc;

namespace backend_planilla.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerarPlanillaController : ControllerBase
    {
        private readonly GenerarPlanilla _generador;
        private readonly ICalculoDeduccionesObligatorias _calculadora;
        private readonly IGetDeduccionBeneficiosQuery _beneficios;

        public GenerarPlanillaController(GenerarPlanilla generador, ICalculoDeduccionesObligatorias calculadora, IGetDeduccionBeneficiosQuery beneficios)
        {
            _generador = generador;
            _calculadora = calculadora;
            _beneficios = beneficios;
        }

        [HttpPost]
        public async Task<IActionResult> Generar([FromBody] GenerarPlanillaRequestModel request)
        {
            var id = await _generador.EjecutarAsync(request, _calculadora, _beneficios);
            return Ok(new { IDPlanilla = id });
        }
    }
}
