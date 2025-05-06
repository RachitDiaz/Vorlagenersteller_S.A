using Microsoft.AspNetCore.Http;
using backend_planilla.Models;
using backend_planilla.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace backend_planilla.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoHandler _handler = new EmpleadoHandler();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_handler.ObtenerEmpleados());
        }

        [HttpPost]
        public IActionResult Post([FromBody] EmpleadoModel nuevo)
        {
            _handler.AgregarEmpleado(nuevo);
            return Ok(new { mensaje = "Empleado agregado correctamente." });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_handler.EliminarEmpleado(id))
                return Ok(new { mensaje = "Empleado eliminado correctamente." });

            return NotFound(new { mensaje = "Empleado no encontrado." });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] EmpleadoModel actualizado)
        {
            actualizado.Id = id;
            if (_handler.EditarEmpleado(actualizado))
                return Ok(new { mensaje = "Empleado actualizado correctamente." });

            return NotFound(new { mensaje = "Empleado no encontrado." });
        }
    }
}
