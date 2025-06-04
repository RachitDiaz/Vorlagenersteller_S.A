using backend_planilla.Domain;
using backend_planilla.Infraestructure;

namespace backend_planilla.Application
{
    public class GenerarPlanillaService : IGenerarPlanillaService
    {
        private readonly IPlanillaRepository _repo;
        private readonly CalcularDeduccionesPlanilla _calculador;

        public GenerarPlanillaService(IPlanillaRepository repo)
        {
            _repo = repo;
            _calculador = new CalcularDeduccionesPlanilla();
        }

        public async Task<bool> GenerarAsync(string cedulaEmpresa, int idPlanilla)
        {
            var empleados = await _repo.ObtenerEmpleadosPorEmpresaAsync(cedulaEmpresa);
            decimal totalSEMPagar = 0, totalSEMDeducir = 0;
            decimal totalIVMPagar = 0, totalIVMDeducir = 0;
            decimal totalLPTPagar = 0, totalLPTDeducir = 0;
            decimal totalRenta = 0, totalBeneficios = 0;
            string periodo = DateTime.Now.ToString("yyyy-MM");

            foreach (var emp in empleados)
            {
                decimal salarioAUsar = emp.TiempoLaboradoDias >= 30
                    ? emp.SalarioBruto
                    : Math.Round(emp.SalarioBruto / 30m * emp.TiempoLaboradoDias, 2);

                var deducciones = _calculador.Calcular(salarioAUsar);
                var beneficios = await _repo.ObtenerMontosBeneficiosEmpleadoAsync(emp.CedulaEmpleado);

                decimal totalEmpleado = deducciones.SEMEmpleado + deducciones.IVMEmpleado + deducciones.LPTEmpleado + deducciones.ImpuestoRenta + beneficios.Sum();
                decimal totalPatrono = deducciones.SEMPatrono + deducciones.IVMPatrono + deducciones.LPTPatrono;

                totalSEMDeducir += deducciones.SEMEmpleado;
                totalSEMPagar += deducciones.SEMPatrono;
                totalIVMDeducir += deducciones.IVMEmpleado;
                totalIVMPagar += deducciones.IVMPatrono;
                totalLPTDeducir += deducciones.LPTEmpleado;
                totalLPTPagar += deducciones.LPTPatrono;
                totalRenta += deducciones.ImpuestoRenta;
                totalBeneficios += beneficios.Sum();

                await _repo.InsertarPlanillaEmpleadoAsync(idPlanilla, emp.CedulaEmpleado, salarioAUsar, deducciones, beneficios, totalEmpleado, totalPatrono);
            }

            return await _repo.InsertarResumenEmpresaAsync(idPlanilla, cedulaEmpresa, periodo,
                totalSEMPagar, totalSEMDeducir, totalIVMPagar, totalIVMDeducir,
                totalLPTPagar, totalLPTDeducir, totalRenta, totalBeneficios);
        }
    }
}
