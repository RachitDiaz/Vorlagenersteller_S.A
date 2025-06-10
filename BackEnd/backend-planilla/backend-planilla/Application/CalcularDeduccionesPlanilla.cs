using backend_planilla.Domain;

namespace backend_planilla.Application
{
    public class CalcularDeduccionesPlanilla
    {
        public DeduccionesPlanillaModel Calcular(decimal salarioBruto)
        {
            // Porcentajes empleados
            decimal porcentajeSEMEmpleado = 0.055m;
            decimal porcentajeIVMEmpleado = 0.0417m;
            decimal porcentajeLPTEmpleado = 0.01m;

            // Porcentajes patrono
            decimal porcentajeSEMPatrono = 0.0925m;
            decimal porcentajeIVMPatrono = 0.0542m;
            decimal porcentajeLPTPatrono = 0.0475m;

            // Cálculo de deducciones
            var result = new DeduccionesPlanillaModel
            {
                SEMEmpleado = Math.Round(salarioBruto * porcentajeSEMEmpleado, 2),
                IVMEmpleado = Math.Round(salarioBruto * porcentajeIVMEmpleado, 2),
                LPTEmpleado = Math.Round(salarioBruto * porcentajeLPTEmpleado, 2),

                SEMPatrono = Math.Round(salarioBruto * porcentajeSEMPatrono, 2),
                IVMPatrono = Math.Round(salarioBruto * porcentajeIVMPatrono, 2),
                LPTPatrono = Math.Round(salarioBruto * porcentajeLPTPatrono, 2),

                ImpuestoRenta = Math.Round(CalcularRenta(salarioBruto), 2)
            };

            return result;
        }

        private decimal CalcularRenta(decimal salario)
        {
            decimal tramo1Limite = 922000;
            decimal tramo2Limite = 1352000;
            decimal tramo3Limite = 2373000;
            decimal tramo4Limite = 4745000;

            decimal porcentaje1 = 0.10m;
            decimal porcentaje2 = 0.15m;
            decimal porcentaje3 = 0.20m;
            decimal porcentaje4 = 0.25m;

            decimal renta = 0;

            if (salario > tramo4Limite)
            {
                renta += (salario - tramo4Limite) * porcentaje4;
                salario = tramo4Limite;
            }

            if (salario > tramo3Limite)
            {
                renta += (salario - tramo3Limite) * porcentaje3;
                salario = tramo3Limite;
            }

            if (salario > tramo2Limite)
            {
                renta += (salario - tramo2Limite) * porcentaje2;
                salario = tramo2Limite;
            }

            if (salario > tramo1Limite)
            {
                renta += (salario - tramo1Limite) * porcentaje1;
            }

            return Math.Round(renta, 2);
        }
    }
}
