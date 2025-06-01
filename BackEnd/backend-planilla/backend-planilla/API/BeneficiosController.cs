using backend_planilla.Application;
using backend_planilla.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace backend_planilla.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiosController : ControllerBase
    {
        private readonly IBeneficiosQuery _beneficiosHandler;
        public BeneficiosController()
        {
            _beneficiosHandler = new BeneficiosQuery();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            Console.WriteLine($"Texto de prueba para ver si sirve el token\n" +
                $" Correo: {correo} acceso en GET /api/beneficios");
            try
            {
                var beneficios = _beneficiosHandler.GetBeneficios(correo);
                Console.WriteLine(beneficios);
                return Ok(beneficios);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Formato inválido: {ex.Message}");
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"No encontrado: {ex.Message}");
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return StatusCode(500, new { mensaje = "Error interno del servidor." });
            }
        }

        [HttpPost]

        public async Task<ActionResult<bool>> CrearBeneficio(BeneficioModel beneficio)
        {
            try
            {
                var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                Console.WriteLine($"Texto de prueba para ver si sirve el token\n " +
                    $" Correo: {correo} acceso en POST /api/beneficios");
                if (beneficio == null)
                {
                    return BadRequest("No se envió ningun beneficio para agregar");
                }

                BeneficiosQuery beneficiosHandler = new();
                var resultado = beneficiosHandler.CrearBeneficio(beneficio, correo);
                return new JsonResult(resultado);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creando beneficio");
            }
        }

    }
}
