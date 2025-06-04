using backend_planilla.Application;
using backend_planilla.Domain;
using Microsoft.AspNetCore.Mvc;

namespace backend_planilla.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeduccionesPlanillaController : ControllerBase
    {
        private readonly CalcularDeduccionesPlanilla _calcular;

        public DeduccionesPlanillaController(CalcularDeduccionesPlanilla calcular)
        {
            _calcular = calcular;
        }

        [HttpGet("{salarioBruto}")]
        public ActionResult<DeduccionesPlanillaModel> Calcular(decimal salarioBruto)
        {
            if (salarioBruto <= 0)
                return BadRequest("El salario debe ser mayor a cero.");

            var resultado = _calcular.Calcular(salarioBruto);
            return Ok(resultado);
        }
    }
}
