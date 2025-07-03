using backend_planilla.Application;
using backend_planilla.Domain;
using backend_planilla.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend_planilla.API
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GenerarPlanillaController : ControllerBase
    {
        private readonly IGenerarPlanilla _generador;
        private readonly ICalculoDeduccionesObligatorias _calculadora;
        private readonly IGetDeduccionBeneficiosQuery _beneficios;
        private readonly IEmpresaRepository _empresaRepository;

        public GenerarPlanillaController(IGenerarPlanilla generador, ICalculoDeduccionesObligatorias calculadora, IGetDeduccionBeneficiosQuery beneficios, IEmpresaRepository empresaRepository)
        {
            _generador = generador;
            _calculadora = calculadora;
            _beneficios = beneficios;
            _empresaRepository = empresaRepository;
        }

        [HttpPost]
        public async Task<IActionResult> GenerarPlanilla()
        {
            try
            {
                var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(correo))
                    return Unauthorized(new { mensaje = "No se pudo obtener el correo desde el token." });

                var cedulaJuridica = _empresaRepository.ObtenerCedulaJuridica(correo);

                if (string.IsNullOrEmpty(cedulaJuridica))
                    return NotFound(new { mensaje = "No se encontró una empresa asociada a este correo." });

                var request = new GenerarPlanillaRequestModel
                {
                    CedulaJuridica = cedulaJuridica
                };

                var id = await _generador.EjecutarAsync(request, _calculadora, _beneficios);
                return Ok(new { IDPlanilla = id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al generar la planilla.", detalle = ex.Message });
            }
        }
    }
}
