using backend_planilla.Application;
using backend_planilla.Domain;
using Microsoft.AspNetCore.Mvc;

namespace backend_planilla.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerarPlanillaController : ControllerBase
    {
        private readonly IGenerarPlanilla _generador;
        private readonly ICalculoDeduccionesObligatorias _calculadora;
        private readonly IGetDeduccionBeneficiosQuery _beneficios;

        public GenerarPlanillaController(IGenerarPlanilla generador, ICalculoDeduccionesObligatorias calculadora, IGetDeduccionBeneficiosQuery beneficios)
        {
            _generador = generador;
            _calculadora = calculadora;
            _beneficios = beneficios;
        }

        [HttpPost]
        public async Task<IActionResult> Generar([FromBody] GenerarPlanillaRequestModel request)
        {
            try
            {
                var id = await _generador.EjecutarAsync(request, _calculadora, _beneficios);
                return Ok(new { IDPlanilla = id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al generar la planilla.", detalle = ex.Message });
            }
        }
    }
}
