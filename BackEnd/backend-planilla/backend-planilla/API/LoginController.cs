using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_planilla.Models;
using backend_planilla.Handlers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend_planilla.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginHandler _loginHandler;
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _loginHandler = new LoginHandler();
            _config = config;
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

            var resultadoValidacion = _loginHandler.ValidarCredenciales(request.Correo, request.Contrasena);

            if (resultadoValidacion.CorreoUsuario != null)
            {
                var rol = resultadoValidacion.EsDueno ? "Dueno" : "Empleado";
                Console.WriteLine(rol);
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, resultadoValidacion.CorreoUsuario),
                    new Claim(ClaimTypes.Role, rol)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_config["Jwt:ExpireHours"])),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { token = tokenString });
            }

            return Unauthorized(new { mensaje = "No hay un usuario registrado con estos datos" });
        }
    }
}
