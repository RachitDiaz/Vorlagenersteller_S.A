using backend_planilla.Handlers;
using backend_planilla.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lista_Beneficios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListaBeneficiosController : ControllerBase
    {
        private readonly BeneficiosHandler _beneficiosHandler;
        public ListaBeneficiosController()
        {
            _beneficiosHandler = new BeneficiosHandler();
        }

        /*
        [HttpGet]
        public List<BeneficioModel> Get()
        {
            var beneficios = _beneficiosHandler.ObtenerBeneficios();
            return beneficios;
        }
        */
        [HttpPost]

        public async Task<ActionResult<bool>> CrearBeneficio(BeneficioModel beneficio)
        {
            try
            {
                if (beneficio == null)
                {
                    return BadRequest();
                }
                BeneficiosHandler beneficiosHandler = new BeneficiosHandler();
                var resultado = beneficiosHandler.CrearBeneficio(beneficio);
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
