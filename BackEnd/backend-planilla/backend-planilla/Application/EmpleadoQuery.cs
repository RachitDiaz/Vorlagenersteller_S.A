using backend_planilla.Domain;
using backend_planilla.Handlers;
using backend_planilla.Infraestructure;
using backend_planilla.Models;

namespace backend_planilla.Application
{
    public class EmpleadoQuery : IEmpleadoQuery
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        public EmpleadoQuery() {
            _empleadoRepository = new EmpleadoRepository();
        }
        public bool CrearEmpleado(PersonaModel persona, EmpleadoModel empleado, string correo)
        {
            return _empleadoRepository.CrearEmpleado(persona, empleado, correo);
        }
        public bool EditarInfoEmpleado(InfoEmpleadoModel datosNuevos, string cedulaEmpleado)
        {
            return _empleadoRepository.EditarInfoEmpleado(datosNuevos, cedulaEmpleado);
        }
        public InfoEmpleadoModel? ObtenerInfoEmpleado(string cedulaEmpleado)
        {
            return _empleadoRepository.ObtenerInfoEmpleado(cedulaEmpleado);
        }
        public List<EmpleadoModel> ObtenerEmpleados(string correo)
        {
            return _empleadoRepository.ObtenerEmpleados(correo);
        }
    }
}
