using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_planilla.Controllers
{
    // Indica que esta clase será un controlador de API y que debe manejar solicitudes HTTP automáticamente
    [ApiController]

    // Define la ruta base del controlador: las solicitudes a /api/login serán dirigidas aquí
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        public class LoginRequest
        {
            public string? Correo { get; set; }
            public string? Contrasena { get; set; } 
            public string? Rol { get; set; }
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            Console.WriteLine($"Intento de login: Correo={request.Correo}, Rol={request.Rol}");

            if (string.IsNullOrWhiteSpace(request.Correo) ||
                string.IsNullOrWhiteSpace(request.Contrasena) ||
                string.IsNullOrWhiteSpace(request.Rol))
            {
                return BadRequest(new { mensaje = "Correo, contraseña y rol son obligatorios." });
            }

            // Simula la verificación del usuario (aquí van las consultas a la base de datos)
            if (request.Correo == "admin@empresa.com" && request.Contrasena == "1234")
            {
                // Devuelve HTTP 200 OK
                return Ok(new { mensaje = $"Bienvenido {request.Rol}" });
            }

            // Error 401 Unauthorized
            return Unauthorized(new { mensaje = "Credenciales incorrectas" });
        }
    }
}
