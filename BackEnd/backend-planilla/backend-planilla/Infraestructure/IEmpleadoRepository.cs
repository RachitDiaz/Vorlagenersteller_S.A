using backend_planilla.Domain;
using backend_planilla.Models;

namespace backend_planilla.Infraestructure
{
    public interface IEmpleadoRepository
    {
        public bool CrearEmpleado(PersonaModel persona, EmpleadoModel empleado, string correo);
        public bool EditarInfoEmpleado(InfoEmpleadoModel datosNuevos, string cedulaEmpleado);
        public InfoEmpleadoModel? ObtenerInfoEmpleado(string cedulaEmpleado);
        public List<EmpleadoModel> ObtenerEmpleados(string correo);
        Task<decimal> ObtenerSalarioBruto(string cedulaEmpleado);
        Task<List<DeduccionBeneficioModel>> ObtenerBeneficiosEmpleado(string cedulaEmpleado);

        public Task<string> ObtenerGeneroEmpleado(string cedulaEmpleado);
    }
}
