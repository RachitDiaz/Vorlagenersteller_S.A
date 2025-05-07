using backend_planilla.Handlers;
using backend_planilla.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Lista_Beneficios.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ListaBeneficiosController : ControllerBase
    {
        private readonly BeneficiosHandler _beneficiosHandler;
        public ListaBeneficiosController()
        {
            _beneficiosHandler = new BeneficiosHandler();
        }

        [HttpGet]
        public List<BeneficioModel> Get()
        {
            var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            Console.WriteLine($"Texto de prueba para ver si sirve el token" +
                $" Correo: {correo} acceso en GET /api/listabeneficios");

            var beneficios = _beneficiosHandler.ObtenerBeneficios(correo);
            return beneficios;
        }

        [HttpPost]

        public async Task<ActionResult<bool>> CrearBeneficio(BeneficioModel beneficio)
        {
            try
            {
                var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                Console.WriteLine($"Texto de prueba para ver si sirve el token" +
                    $" Correo: {correo} acceso en POST /api/listabeneficios");
                if (beneficio == null)
                {
                    return BadRequest();
                }

                BeneficiosHandler beneficiosHandler = new BeneficiosHandler();
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
