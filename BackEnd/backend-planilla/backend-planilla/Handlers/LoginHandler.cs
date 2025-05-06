namespace backend_planilla.Handlers
{
    public class LoginHandler
    {
        public bool ValidarCredenciales(string correo, string contrasena)
        {
            // Aquí iría una consulta real a una base de datos
            // Por ahora se simula con valores quemados
            return correo == "admin@empresa.com" && contrasena == "1234";
        }
    }
}
