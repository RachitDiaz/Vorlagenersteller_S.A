using backend_planilla.Handlers;
using backend_planilla.Domain;
using backend_planilla.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend_planilla.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesQuery _ReportesQuery;
        private readonly IEmpleadoQuery _EmpleadoQuery;
        private readonly DuenoHandler _DuenoHandler;
        public ReportesController()
        {
            _ReportesQuery = new ReportesQuery();
            _EmpleadoQuery = new EmpleadoQuery();
            _DuenoHandler = new DuenoHandler();
        }

        [HttpGet]
        public IActionResult ObtenerUltimosPagosEmpleado()
        {
            string _correoEmpleado = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(_correoEmpleado))
                return BadRequest();

            string _cedulaEmpleado;
            List<ReportePagoEmpleadoDTO> _resultado;

            try
            {
                _cedulaEmpleado = _EmpleadoQuery.ObtenerCedulaEmpleado(_correoEmpleado);
            }
            catch (Exception mensajeError)
            {
                return BadRequest("Error recuperando cedula del empleado");
            }

            try
            {
                _resultado = _ReportesQuery.ObtenerUltimosPagosEmpleado(_cedulaEmpleado);

            }
            catch (Exception mensajeError)
            {
                return BadRequest("Error recuperando pagos al empleado");
            }

            return Ok(_resultado);
        }

        [HttpGet]
        public IActionResult ObtenerUltimosPagosEmpresa()
        {
            string _correoDueno = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(_correoDueno))
                return BadRequest();

            string _cedulaDueno;
            List<ReportePagoEmpresaDTO> _resultado;

            try
            {
                _cedulaDueno = _DuenoHandler.ObtenerCedulaDueno(_correoDueno);
            }
            catch (Exception mensajeError)
            {
                return BadRequest("Error recuperando cedula del empleado");
            }

            try
            {
                _resultado = _ReportesQuery.ObtenerUltimosPagosEmpresa(_cedulaDueno);
            }
            catch (Exception mensajeError)
            {
                return BadRequest("Error recuperando pagos al empleado");
            }

            return Ok(_resultado);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> enviarEmailReporte(IFormFile documentoPDF)
        {
            try
            {
                string _correoUsuario = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (string.IsNullOrWhiteSpace(_correoUsuario))
                    return BadRequest();

                _correoUsuario = "cascante.aldo@gmail.com";
                _ReportesQuery.enviarEmailReporte(documentoPDF, _correoUsuario);
                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creando empleado.");
            }
            return true;
        }

    }
        
}
