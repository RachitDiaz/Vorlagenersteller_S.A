using backend_planilla.Application;
using backend_planilla.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using backend_planilla.Exceptions;

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

            try
            {
                var beneficios = _beneficiosHandler.GetBeneficios(correo);
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

        [HttpPost("Agregar")]
        public async Task<ActionResult<bool>> CrearBeneficio([FromBody] BeneficioModel beneficio)
        {
            try
            {
                var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (beneficio == null)
                {
                    return BadRequest("No se envió ningun beneficio para agregar");
                }

                BeneficiosQuery beneficiosHandler = new();
                var resultado = beneficiosHandler.CrearBeneficio(beneficio, correo);
                return new JsonResult(resultado);
            }
            catch (ResourceAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creando beneficio. Intente de nuevo");
            }
        }

        [HttpPost("Modificar")]
        public async Task<ActionResult<bool>> ModificarBeneficio([FromBody] ModificarBeneficioModel Beneficios)
        {
            try
            {
                var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                BeneficiosQuery beneficiosHandler = new();
                var resultado = beneficiosHandler.ModificarBeneficio(Beneficios.modificado, Beneficios.original, correo);
                return new JsonResult(resultado);
            }
            catch (ResourceAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creando beneficio. Intente de nuevo");
            }
        }

    }
}
