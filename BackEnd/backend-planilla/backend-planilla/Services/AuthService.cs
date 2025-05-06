namespace backend_planilla.Services
{
    public class AuthService
    {
        public bool ValidarCredenciales(string correo, string contrasena)
        {
            // Aquí se consulta la base de datos
            return correo == "admin@empresa.com" && contrasena == "1234";
        }
    }
}
