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

        [HttpGet("{cedula}")]
        public async Task<ActionResult<List<DeduccionCalculada>>> ObtenerDeducciones(string cedula)
        {
            if (string.IsNullOrEmpty(cedula))
                return Unauthorized("No se encontró el correo en el token de autenticación.");

            var resultado = await _query.CalcularDeduccionesBeneficios(cedula);

            return Ok(resultado);
        }
    }
}