using backend_planilla.Domain;

namespace backend_planilla.Application
{
    public class CalculoDeduccionesObligatorias
    {
        private const decimal porcentajeSEMEmpleado = 0.055m;
        private const decimal porcentajeIVMEmpleado = 0.0417m;
        private const decimal porcentajeBPOPEmpleado = 0.01m;

        private const decimal porcentajeSEMPatrono = 0.0925m;
        private const decimal porcentajeIVMPatrono = 0.0542m;
        private const decimal porcentajeBPOPPatrono = 0.005m;
        private const decimal porcentajeAsignacionesFamiliaresPatrono = 0.05m;
        private const decimal porcentajeIMASPatrono = 0.005m;
        private const decimal porcentajeINAPatrono = 0.0150m;
        private const decimal porcentajeFCLPatrono = 0.0150m;
        private const decimal porcentajeOPCPatrono = 0.02m;
        private const decimal porcentajeINSPatrono = 0.01m;

        private const decimal tramo1Limite = 922000;
        private const decimal tramo2Limite = 1352000;
        private const decimal tramo3Limite = 2373000;
        private const decimal tramo4Limite = 4745000;

        private const decimal porcentaje1 = 0.10m;
        private const decimal porcentaje2 = 0.15m;
        private const decimal porcentaje3 = 0.20m;
        private const decimal porcentaje4 = 0.25m;

        public DeduccionesObligatoriasModel CalculoMensual(decimal salarioBruto)
        {
            var resultado = new DeduccionesObligatoriasModel
            {
                SEMEmpleado = Math.Round(salarioBruto * porcentajeSEMEmpleado, 2),
                IVMEmpleado = Math.Round(salarioBruto * porcentajeIVMEmpleado, 2),
                BPPOEmpleado = Math.Round(salarioBruto * porcentajeBPOPEmpleado, 2),
                ImpuestoRenta = Math.Round(CalcularRenta(salarioBruto), 2),

                SEMPatrono = Math.Round(salarioBruto * porcentajeSEMPatrono, 2),
                IVMPatrono = Math.Round(salarioBruto * porcentajeIVMPatrono, 2),
                BPOPPatrono = Math.Round(salarioBruto * porcentajeBPOPPatrono, 2),
                AsignacionesFamiliaresPatrono = Math.Round(salarioBruto * porcentajeAsignacionesFamiliaresPatrono, 2),
                IMASPatrono = Math.Round(salarioBruto * porcentajeIMASPatrono, 2),
                INAPatrono = Math.Round(salarioBruto * porcentajeINAPatrono, 2),
                FCLPatrono = Math.Round(salarioBruto * porcentajeFCLPatrono, 2),
                OPCPatrono = Math.Round(salarioBruto * porcentajeOPCPatrono, 2),
                INSPatrono = Math.Round(salarioBruto * porcentajeINSPatrono, 2)
            };

            return resultado;
        }

        public DeduccionesObligatoriasModel CalculoQuincenal(decimal salarioBruto)
        {
            var resultado = CalculoMensual(salarioBruto);

            return new DeduccionesObligatoriasModel
            {
                SEMEmpleado = Math.Round(resultado.SEMEmpleado / 2, 2),
                IVMEmpleado = Math.Round(resultado.IVMEmpleado / 2, 2),
                BPPOEmpleado = Math.Round(resultado.BPPOEmpleado / 2, 2),
                ImpuestoRenta = Math.Round(resultado.ImpuestoRenta / 2, 2),

                SEMPatrono = Math.Round(resultado.SEMPatrono / 2, 2),
                IVMPatrono = Math.Round(resultado.IVMPatrono / 2, 2),
                BPOPPatrono = Math.Round(resultado.BPOPPatrono / 2, 2),
                AsignacionesFamiliaresPatrono = Math.Round(resultado.AsignacionesFamiliaresPatrono / 2, 2),
                IMASPatrono = Math.Round(resultado.IMASPatrono / 2, 2),
                INAPatrono = Math.Round(resultado.INAPatrono / 2, 2),
                FCLPatrono = Math.Round(resultado.FCLPatrono / 2, 2),
                OPCPatrono = Math.Round(resultado.OPCPatrono / 2, 2),
                INSPatrono = Math.Round(resultado.INSPatrono / 2, 2)
            };
        }

        private decimal CalcularRenta(decimal salario)
        {
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
