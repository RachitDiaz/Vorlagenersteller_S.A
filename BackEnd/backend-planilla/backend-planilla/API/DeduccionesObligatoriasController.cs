using backend_planilla.Application;
using backend_planilla.Domain;
using Microsoft.AspNetCore.Mvc;

namespace backend_planilla.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeduccionesObligatoriasController : ControllerBase
    {
        private readonly CalculoDeduccionesObligatorias _calcular;

        public DeduccionesObligatoriasController(CalculoDeduccionesObligatorias calcular)
        {
            _calcular = calcular;
        }

        [HttpGet("mensual/{salarioBruto}")]
        public ActionResult<DeduccionesObligatoriasModel> CalcularMensual(decimal salarioBruto)
        {
            if (salarioBruto <= 0)
                return BadRequest("El salario debe ser mayor a cero.");

            var resultado = _calcular.CalculoMensual(salarioBruto);
            return Ok(resultado);
        }

        [HttpGet("quincenal/{salarioBruto}")]
        public ActionResult<DeduccionesObligatoriasModel> CalcularQuincenal(decimal salarioBruto)
        {
            if (salarioBruto <= 0)
                return BadRequest("El salario debe ser mayor a cero.");

            var resultado = _calcular.CalculoQuincenal(salarioBruto);
            return Ok(resultado);
        }
    }
}
