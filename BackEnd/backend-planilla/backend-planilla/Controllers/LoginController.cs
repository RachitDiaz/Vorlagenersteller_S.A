using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_planilla.Models;
using backend_planilla.Handlers;

namespace backend_planilla.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginHandler _loginHandler;

        public LoginController()
        {
            _loginHandler = new LoginHandler();
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Correo) ||
                string.IsNullOrWhiteSpace(request.Contrasena) ||
                string.IsNullOrWhiteSpace(request.Rol))
            {
                return BadRequest(new { mensaje = "Correo, contraseña y rol son obligatorios." });
            }

            if (_loginHandler.ValidarCredenciales(request.Correo, request.Contrasena))
            {
                return Ok(new { mensaje = $"Bienvenido {request.Rol}" });
            }

            return Unauthorized(new { mensaje = "Credenciales incorrectas" });
        }
    }
}
