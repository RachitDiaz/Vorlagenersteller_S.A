using System.Security.Claims;
using backend_planilla.Application;
using backend_planilla.Domain;
using Microsoft.AspNetCore.Mvc;

namespace backend_planilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficioEmpleadoController : ControllerBase
    {
        private readonly IBeneficioQuery _beneficioQuery;

        public BeneficioEmpleadoController()
        {
            _beneficioQuery = new BeneficioQuery();
        }


        [HttpPost("actualizar")]
        public IActionResult ActualizarBeneficios([FromBody] Dictionary<string, List<int>> body)
        {
            if (body == null || !body.ContainsKey("beneficios"))
                return BadRequest("Lista de beneficios vacía.");

            var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (correo == null)
                return Unauthorized("Token inválido.");

            var cedulaEmpleado = _beneficioQuery.ObtenerCedulaEmpleadoDesdeCorreo(correo);
            if (cedulaEmpleado == null)
                return NotFound("No se encontró un empleado con ese correo.");

            var beneficios = body["beneficios"];
            var resultado = _beneficioQuery.ActualizarBeneficiosEmpleado(cedulaEmpleado, beneficios);
            return Ok(new { exito = resultado });
        }


        [HttpGet("listar")]
        public IActionResult ObtenerBeneficiosDisponibles()
        {
            try
            {
                var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                Console.WriteLine(correo);
                if (string.IsNullOrWhiteSpace(correo))
                    return Unauthorized("Token no válido o correo no encontrado.");

                var beneficiosDisponibles = _beneficioQuery.ObtenerBeneficiosParaEmpleado(correo);
                return Ok(beneficiosDisponibles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en listar: {ex.Message}");
                return StatusCode(500, $"Error al obtener beneficios disponibles: {ex.Message}");
            }
        }


        [HttpGet("elegidos")]
        public IActionResult ObtenerBeneficiosSeleccionados()
        {
            try
            {
                var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                Console.WriteLine(correo);
                if (string.IsNullOrWhiteSpace(correo))
                    return Unauthorized("Token no válido o correo no encontrado.");

                var beneficiosSeleccionados = _beneficioQuery.ObtenerBeneficiosSeleccionadosPorEmpleado(correo);

                return Ok(beneficiosSeleccionados);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en elegidos: {ex.Message}");
                return StatusCode(500, $"Error al obtener beneficios seleccionados: {ex.Message}");
            }
        }
    }
}
