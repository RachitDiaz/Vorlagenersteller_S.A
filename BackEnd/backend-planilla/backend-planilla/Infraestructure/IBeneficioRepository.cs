using backend_planilla.Domain;

namespace backend_planilla.Infraestructure
{
    public interface IBeneficioRepository
    {
        bool ActualizarBeneficiosEmpleado(string cedulaEmpleado, List<int> beneficios);

        List<BeneficioSimpleModel> ObtenerBeneficiosParaEmpleado(string correo);

        List<BeneficioSimpleModel> ObtenerBeneficiosSeleccionadosPorEmpleado(string correo);

        public bool ActualizarDependientesEmpleado(string cedula, int dependientes);
        string ObtenerCedulaEmpleadoDesdeCorreo(string correo);
    }
}
