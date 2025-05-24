namespace backend_planilla.Models
{
    public class LoginRequestModel
    {
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
        public string? Rol { get; set; }
    }
}
