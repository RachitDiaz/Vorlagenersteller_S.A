using backend_planilla.Models;

namespace backend_planilla.Models
{
    public class PersonaModel
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Genero { get; set; }
        public UsuarioModel Usuario { get; set; }
    }
}
