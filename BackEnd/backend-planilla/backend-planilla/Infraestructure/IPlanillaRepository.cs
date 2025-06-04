using backend_planilla.Domain;

namespace backend_planilla.Infraestructure
{
    public interface IPlanillaRepository
    {
        Task<List<PlanillaMensualEmpleadoModel>> ObtenerEmpleadosPorEmpresaAsync(string cedulaEmpresa);
        Task<List<decimal>> ObtenerMontosBeneficiosEmpleadoAsync(string cedulaEmpleado);
        Task<bool> InsertarPlanillaEmpleadoAsync(int idPlanilla, string cedulaEmpleado, decimal salarioBruto, DeduccionesPlanillaModel deducciones, List<decimal> beneficios, decimal totalEmpleado, decimal totalPatrono);
        Task<bool> InsertarResumenEmpresaAsync(int idPlanilla, string cedulaEmpresa, string periodo,
            decimal totalSEMPagar, decimal totalSEMDeducir, decimal totalIVMPagar, decimal totalIVMDeducir,
            decimal totalLPTPagar, decimal totalLPTDeducir, decimal totalRenta, decimal totalBeneficios);
    }
}
