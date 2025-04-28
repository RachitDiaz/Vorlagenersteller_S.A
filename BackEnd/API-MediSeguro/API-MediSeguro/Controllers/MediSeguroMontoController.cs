using API_MediSeguro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_MediSeguro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediSeguroMontoController : ControllerBase
    {
        private const string TokenEsperado = "TOKEN123"; // Token quemado

        [HttpPost]
        public IActionResult CalcularMonto([FromHeader] string token, [FromBody] SolicitudMediSeguroMontoModel solicitud)
        {
            if (token != TokenEsperado)
                return StatusCode(403, "Token inválido.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 si el modelo no es válido

            int edad = CalcularEdad(solicitud.FechaNacimiento);

            decimal montoBase;
            string genero = solicitud.Genero.ToLower();

            if (genero == "femenino")
            {
                montoBase = CalcularMontoMujer(edad);
            }
            else if (genero == "masculino")
            {
                montoBase = CalcularMontoHombre(edad);
            }
            else
            {
                return BadRequest("Género inválido. Debe ser 'femenino' o 'masculino'.");
            }

            montoBase += solicitud.CantidadDependientes * 50000;

            return Ok(montoBase);
        }

        private int CalcularEdad(DateTime fechaNacimiento)
        {
            int edad = DateTime.Today.Year - fechaNacimiento.Year;
            if (fechaNacimiento.Date > DateTime.Today.AddYears(-edad))
                edad--;
            return edad;
        }

        private decimal CalcularMontoMujer(int edad)
        {
            if (edad < 30)
                return 75000;
            else if (edad <= 50)
                return 100000;
            else
                return 140000;
        }

        private decimal CalcularMontoHombre(int edad)
        {
            if (edad < 30)
                return 90000;
            else if (edad <= 50)
                return 115000;
            else
                return 155000;
        }
    }
}
