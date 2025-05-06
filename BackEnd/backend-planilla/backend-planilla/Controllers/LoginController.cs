using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_planilla.Models;
using backend_planilla.Services;

namespace backend_planilla.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AuthService _authService;

        public LoginController()
        {
            _authService = new AuthService(); // Idealmente usar inyección de dependencias
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestModel request)
        {
            Console.WriteLine($"Intento de login: Correo={request.Correo}, Rol={request.Rol}");

            if (string.IsNullOrWhiteSpace(request.Correo) ||
                string.IsNullOrWhiteSpace(request.Contrasena) ||
                string.IsNullOrWhiteSpace(request.Rol))
            {
                return BadRequest(new { mensaje = "Correo, contraseña y rol son obligatorios." });
            }

            if (_authService.ValidarCredenciales(request.Correo, request.Contrasena))
            {
                return Ok(new { mensaje = $"Bienvenido {request.Rol}" });
            }

            return Unauthorized(new { mensaje = "Credenciales incorrectas" });
        }
    }
}
