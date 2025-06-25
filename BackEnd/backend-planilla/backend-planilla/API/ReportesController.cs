using backend_planilla.Handlers;
using backend_planilla.Models;
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
        public ReportesController()
        {
            _ReportesQuery = new ReportesQuery();
            _EmpleadoQuery = new EmpleadoQuery();
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

    }
        
}
