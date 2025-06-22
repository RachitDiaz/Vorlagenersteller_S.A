using backend_planilla.Handlers;
using backend_planilla.Models;
using backend_planilla.Domain;
using backend_planilla.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend_planilla.Controllers
{
    //[Authorize]
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
        public IActionResult ObtenerUltimosPagosEmpleado(string correoEmpleado)
        {
            if (string.IsNullOrWhiteSpace(correoEmpleado))
                return BadRequest();

            string _cedulaEmpleado;
            List<ReportePagoEmpleadoDTO> _resultado;

            try
            {
                _cedulaEmpleado = _EmpleadoQuery.ObtenerCedulaEmpleado(correoEmpleado);
                _resultado = _ReportesQuery.ObtenerUltimosPagosEmpleado(_cedulaEmpleado);
            }
            catch (Exception mensajeError)
            {
                return BadRequest("Error recuperando pagos al empleado");
            }

            return Ok(_resultado);
        }

        /*
        [HttpGet]
        public async Task<IActionResult> GetDeducciones([FromQuery] string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
                return BadRequest("Debe indicar una cédula.");

            var resultado = await _query.ExecuteAsync(cedula);
            return Ok(resultado);
        }

        [HttpGet]
        public List<EmpleadoModel> GetEmpleadosEmpresa()
        {
            var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            Console.WriteLine($"Texto de prueba para ver si sirve el token" +
                $" Correo: {correo} acceso en GET /api/empleadoController");

            var empleados = _empleadoHandler.ObtenerEmpleados(correo);
            return empleados;
        }

        [HttpGet]
        public InfoEmpleadoModel? GetInfoEmpleado(string cedulaEmpleado)
        {
            var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var infoEmpleado = _empleadoHandler.ObtenerInfoEmpleado(cedulaEmpleado);
            return infoEmpleado;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CrearEmpleado(SolicitudAgregarEmpleadoModel paqueteSolicitudAgregarEmpleado)
        {
            try
            {
                var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                PersonaModel persona = paqueteSolicitudAgregarEmpleado.Persona;
                EmpleadoModel empleado = paqueteSolicitudAgregarEmpleado.Empleado;
                Console.WriteLine($"Texto de prueba para ver si sirve el token" +
                    $" Correo: {correo} acceso en POST /api/empleadoController");
                if (persona == null || empleado == null)
                {
                    return BadRequest();
                }   

                EmpleadoRepository empleadoHandler = new EmpleadoRepository();
                var resultado = empleadoHandler.CrearEmpleado(persona, empleado, correo);
                return new JsonResult(resultado);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creando empleado.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> EditarInfoEmpleado(SolicitudEditarEmpleadoModel SolicitudEditarEmpleado)
        {
            try
            {
                var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                InfoEmpleadoModel informacionNueva = SolicitudEditarEmpleado.InfoEmpleado;
                string cedulaEmpleado = SolicitudEditarEmpleado.CedulaAEditar;

                if (informacionNueva == null || cedulaEmpleado == null)
                {
                    return BadRequest();
                }

                EmpleadoRepository empleadoHandler = new EmpleadoRepository();
                var resultado = empleadoHandler.EditarInfoEmpleado(informacionNueva, cedulaEmpleado);
                return new JsonResult(resultado);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error editando empleado.");
            }
        }*/

    }
        
}
