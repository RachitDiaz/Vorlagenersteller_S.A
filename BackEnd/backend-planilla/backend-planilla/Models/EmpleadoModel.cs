namespace backend_planilla.Models
{
    public class EmpleadoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido1 { get; set; } = "";
        public string Apellido2 { get; set; } = "";
        public string Cedula { get; set; } = "";
        public string Posicion { get; set; } = "";
    }
}
