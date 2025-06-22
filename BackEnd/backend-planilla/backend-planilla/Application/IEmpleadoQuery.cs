using backend_planilla.Domain;
using backend_planilla.Infraestructure;
using backend_planilla.Models;

namespace backend_planilla.Application
{
    public interface IEmpleadoQuery
    {
        public bool CrearEmpleado(PersonaModel persona, EmpleadoModel empleado, string correo);
        public bool EditarInfoEmpleado(InfoEmpleadoModel datosNuevos, string cedulaEmpleado);
        public InfoEmpleadoModel? ObtenerInfoEmpleado(string cedulaEmpleado);
        public List<EmpleadoModel> ObtenerEmpleados(string correo);
        public string ObtenerCedulaEmpleado(string correo);
    }
}
