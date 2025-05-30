using backend_planilla.Models;

namespace backend_planilla.Domain
{
    public class InfoEmpleadoModel
    {
        public EmpleadoModel Empleado { get; set; }
        public string Genero { get; set; }
        public string Correo { get; set; }
    }

    public class SolicitudEditarEmpleadoModel
    {
        public InfoEmpleadoModel InfoEmpleado { get; set; }
        public string CedulaAEditar { get; set; }
    }
}
