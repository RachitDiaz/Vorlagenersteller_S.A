using backend_planilla.Handlers;
using backend_planilla.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace backend_planilla.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoHandler _empleadoHandler;
        public EmpleadoController()
        {
            _empleadoHandler = new EmpleadoHandler();
        }

        [HttpGet]
        public List<EmpleadoModel> Get()
        {
            //var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            //Console.WriteLine($"Texto de prueba para ver si sirve el token" +
                //$" Correo: {correo} acceso en GET /api/empleadoController");

            var empleados = _empleadoHandler.ObtenerEmpleados("shihtangdaniel@gmail.com"/*correo*/);
            return empleados;
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
                    "Error creando beneficio");
            }
        }

    }
}
