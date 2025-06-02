using backend_planilla.Handlers;
using backend_planilla.Models;
using backend_planilla.Domain;
using backend_planilla.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace backend_planilla.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoQuery _empleadoHandler;
        public EmpleadoController()
        {
            _empleadoHandler = new EmpleadoQuery();
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

                EmpleadoHandler empleadoHandler = new EmpleadoHandler();
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

                EmpleadoHandler empleadoHandler = new EmpleadoHandler();
                var resultado = empleadoHandler.EditarInfoEmpleado(informacionNueva, cedulaEmpleado);
                return new JsonResult(resultado);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error editando empleado.");
            }
        }

    }
}
