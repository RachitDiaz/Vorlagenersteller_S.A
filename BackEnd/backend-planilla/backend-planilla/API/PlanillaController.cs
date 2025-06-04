using backend_planilla.Application;
using Microsoft.AspNetCore.Mvc;

namespace backend_planilla.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanillaController : ControllerBase
    {
        private readonly IGenerarPlanillaService _service;

        public PlanillaController(IGenerarPlanillaService service)
        {
            _service = service;
        }

        [HttpPost("generar")]
        public async Task<IActionResult> Generar([FromQuery] string cedulaEmpresa, [FromQuery] int idPlanilla)
        {
            if (string.IsNullOrWhiteSpace(cedulaEmpresa))
                return BadRequest("Datos incompletos");

            var result = await _service.GenerarAsync(cedulaEmpresa, idPlanilla);
            return result ? Ok("Planilla generada correctamente") : StatusCode(500, "Error generando la planilla");
        }
    }
}
