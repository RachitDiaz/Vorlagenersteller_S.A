using backend_planilla.Handlers;

namespace backend_planilla.Services
{
    public class LoginService
    {
        private readonly LoginHandler _authHandler;

        public LoginService()
        {
            _authHandler = new LoginHandler(); // Idealmente esto también va por inyección
        }

        public bool ValidarCredenciales(string correo, string contrasena)
        {
            return _authHandler.ConsultarUsuarioEnBaseDeDatos(correo, contrasena);
        }
    }
}
