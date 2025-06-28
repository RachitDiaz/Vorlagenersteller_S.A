using backend_planilla.Application;
using backend_planilla.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend_planilla.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeduccionesBeneficiosController : ControllerBase
    {
        private readonly IGetDeduccionBeneficiosQuery _query;

        public DeduccionesBeneficiosController(IGetDeduccionBeneficiosQuery query)
        {
            _query = query;
        }

        // GET: api/DeduccionesBeneficios
        [HttpGet]
        [Authorize] // Asegura que solo usuarios autenticados puedan acceder
        public async Task<ActionResult<List<DeduccionCalculada>>> ObtenerDeducciones()
        {
            // Extraer el correo electrónico del claim
            var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            Console.WriteLine(correo);
            if (string.IsNullOrEmpty(correo))
                return Unauthorized("No se encontró el correo en el token de autenticación.");

            var resultado = await _query.CalcularDeduccioensBeneficios(correo);

            return Ok(resultado);
        }
    }
}