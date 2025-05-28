using backend_planilla.Handlers;
using backend_planilla.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_planilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DuenoController : ControllerBase
    {
        private readonly DuenoHandler _duenoHandler;

        public DuenoController()
        {
            _duenoHandler = new DuenoHandler();
        }

        [HttpPost]
        public ActionResult<bool> CrearDueno([FromBody] DuenoModel dueno)
        {
            try
            {
                Console.WriteLine("si llego al post");
                if (dueno == null || dueno.Persona == null || dueno.Persona.Usuario == null)
                    return BadRequest();
                    
                var resultado = _duenoHandler.CrearDueno(dueno);
                return new JsonResult(resultado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                return StatusCode(500, $"Error al crear dueño: {ex.Message}");
            }
        }
    }
}
