using backend_planilla.Domain;

namespace backend_planilla.Application
{
    public class CalculoDeduccionesObligatorias
    {
        private readonly decimal porcentajeSEMEmpleado = 0.055m;
        private readonly decimal porcentajeIVMEmpleado = 0.0417m;
        private readonly decimal porcentajeBPOPEmpleado = 0.01m;

        private readonly decimal porcentajeSEMPatrono = 0.0925m;
        private readonly decimal porcentajeIVMPatrono = 0.0542m;
        private readonly decimal porcentajeBPOPPatrono = 0.005m;
        private readonly decimal porcentajeAsignacionesFamiliaresPatrono = 0.05m;
        private readonly decimal porcentajeIMASPatrono = 0.005m;
        private readonly decimal porcentajeINAPatrono = 0.0150m;
        private readonly decimal porcentajeFCLPatrono = 0.0150m;
        private readonly decimal porcentajeOPCPatrono = 0.02m;
        private readonly decimal porcentajeINSPatrono = 0.01m;

        public DeduccionesObligatoriasModel CalculoMensual(decimal salarioBruto)
        {
            var result = new DeduccionesObligatoriasModel
            {
                SEMEmpleado = Math.Round(salarioBruto * porcentajeSEMEmpleado, 2),
                IVMEmpleado = Math.Round(salarioBruto * porcentajeIVMEmpleado, 2),
                BPPOEmpleado = Math.Round(salarioBruto * porcentajeBPOPEmpleado, 2),

                SEMPatrono = Math.Round(salarioBruto * porcentajeSEMPatrono, 2),
                IVMPatrono = Math.Round(salarioBruto * porcentajeIVMPatrono, 2),
                BPOPPatrono = Math.Round(salarioBruto * porcentajeBPOPPatrono, 2),
                AsignacionesFamiliaresPatrono = Math.Round(salarioBruto * porcentajeAsignacionesFamiliaresPatrono, 2),
                IMASPatrono = Math.Round(salarioBruto * porcentajeIMASPatrono, 2),
                INAPatrono = Math.Round(salarioBruto * porcentajeINAPatrono, 2),
                FCLPatrono = Math.Round(salarioBruto * porcentajeFCLPatrono, 2),
                OPCPatrono = Math.Round(salarioBruto * porcentajeOPCPatrono, 2),
                INSPatrono = Math.Round(salarioBruto * porcentajeINSPatrono, 2),

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
