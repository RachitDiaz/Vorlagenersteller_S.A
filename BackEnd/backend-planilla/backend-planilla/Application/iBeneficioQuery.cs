using backend_planilla.Domain;

namespace backend_planilla.Application
{
    public interface IBeneficioQuery
    {
        bool ActualizarBeneficiosEmpleado(string cedulaEmpleado, List<int> beneficios);

        List<BeneficioSimpleModel> ObtenerBeneficiosParaEmpleado(string correo);

        List<BeneficioSimpleModel> ObtenerBeneficiosSeleccionadosPorEmpleado(string correo);
    }
}