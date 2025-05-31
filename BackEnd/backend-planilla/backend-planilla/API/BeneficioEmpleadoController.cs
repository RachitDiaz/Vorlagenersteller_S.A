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
        public IActionResult ActualizarBeneficios([FromBody] BeneficioEmpleadoModel model)
        {
            if (model == null || model.CedulaEmpleado == null || model.Beneficios == null)
                return BadRequest("Datos incompletos.");

            var resultado = _beneficioQuery.ActualizarBeneficiosEmpleado(model.CedulaEmpleado, model.Beneficios);
            return Ok(new { exito = resultado });
        }

        [HttpGet("listar")]
        public IActionResult ObtenerBeneficiosDisponibles()
        {
            try
            {
                var correo = User.Identity?.Name; // Asegúrate de que el JWT tenga el claim del correo en `sub` o `email`
                if (correo == null)
                    return Unauthorized("Token no válido o correo no encontrado.");

                var lista = _beneficioQuery.ObtenerBeneficiosParaEmpleado(correo);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener beneficios: {ex.Message}");
            }
        }

        [HttpGet("elegidos")]
        public IActionResult ObtenerBeneficiosSeleccionados()
        {
            try
            {
                var correo = User.Identity?.Name;
                if (correo == null)
                    return Unauthorized("Token no válido o correo no encontrado.");

                var beneficios = _beneficioQuery.ObtenerBeneficiosSeleccionadosPorEmpleado(correo);
                return Ok(beneficios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener beneficios seleccionados: {ex.Message}");
            }
        }
    }
}
