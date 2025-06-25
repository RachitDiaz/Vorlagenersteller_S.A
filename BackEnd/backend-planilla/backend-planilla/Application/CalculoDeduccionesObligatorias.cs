using backend_planilla.Domain;

namespace backend_planilla.Application
{
    public class CalculoDeduccionesObligatorias : ICalculoDeduccionesObligatorias
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

        private const decimal rentaTramo1Limite = 922000;
        private const decimal rentaTramo2Limite = 1352000;
        private const decimal rentaTramo3Limite = 2373000;
        private const decimal rentaTramo4Limite = 4745000;

        private const decimal rentaPorcentaje1 = 0.10m;
        private const decimal rentaPorcentaje2 = 0.15m;
        private const decimal rentaPorcentaje3 = 0.20m;
        private const decimal rentaPorcentaje4 = 0.25m;

        private const int cantidadDecimales = 2;
        private const int divisorQuincenal = 2;

        public DeduccionesObligatoriasModel CalcularDeduccionMensual(decimal salarioBruto)
        {
            return new DeduccionesObligatoriasModel
            {
                SEMEmpleado = Math.Round(salarioBruto * porcentajeSEMEmpleado, cantidadDecimales),
                IVMEmpleado = Math.Round(salarioBruto * porcentajeIVMEmpleado, cantidadDecimales),
                BPPOEmpleado = Math.Round(salarioBruto * porcentajeBPOPEmpleado, cantidadDecimales),
                ImpuestoRenta = Math.Round(CalcularRenta(salarioBruto), cantidadDecimales),

                SEMPatrono = Math.Round(salarioBruto * porcentajeSEMPatrono, cantidadDecimales),
                IVMPatrono = Math.Round(salarioBruto * porcentajeIVMPatrono, cantidadDecimales),
                BPOPPatrono = Math.Round(salarioBruto * porcentajeBPOPPatrono, cantidadDecimales),
                AsignacionesFamiliaresPatrono = Math.Round(salarioBruto * porcentajeAsignacionesFamiliaresPatrono, cantidadDecimales),
                IMASPatrono = Math.Round(salarioBruto * porcentajeIMASPatrono, cantidadDecimales),
                INAPatrono = Math.Round(salarioBruto * porcentajeINAPatrono, cantidadDecimales),
                FCLPatrono = Math.Round(salarioBruto * porcentajeFCLPatrono, cantidadDecimales),
                OPCPatrono = Math.Round(salarioBruto * porcentajeOPCPatrono, cantidadDecimales),
                INSPatrono = Math.Round(salarioBruto * porcentajeINSPatrono, cantidadDecimales)
            };
        }

        public DeduccionesObligatoriasModel CalcularDeduccionQuincenal(decimal salarioBruto)
        {
            var mensual = CalcularDeduccionMensual(salarioBruto);

            return new DeduccionesObligatoriasModel
            {
                SEMEmpleado = mensual.SEMEmpleado / divisorQuincenal,
                IVMEmpleado = mensual.IVMEmpleado / divisorQuincenal,
                BPPOEmpleado = mensual.BPPOEmpleado / divisorQuincenal,
                ImpuestoRenta = mensual.ImpuestoRenta / divisorQuincenal,

                SEMPatrono = mensual.SEMPatrono / divisorQuincenal,
                IVMPatrono = mensual.IVMPatrono / divisorQuincenal,
                BPOPPatrono = mensual.BPOPPatrono / divisorQuincenal,
                AsignacionesFamiliaresPatrono = mensual.AsignacionesFamiliaresPatrono / divisorQuincenal,
                IMASPatrono = mensual.IMASPatrono / divisorQuincenal,
                INAPatrono = mensual.INAPatrono / divisorQuincenal,
                FCLPatrono = mensual.FCLPatrono / divisorQuincenal,
                OPCPatrono = mensual.OPCPatrono / divisorQuincenal,
                INSPatrono = mensual.INSPatrono / divisorQuincenal
            };
        }

        private decimal CalcularRenta(decimal salario)
        {
            decimal renta = 0;

            if (salario > rentaTramo4Limite)
            {
                renta += (salario - rentaTramo4Limite) * rentaPorcentaje4;
                salario = rentaTramo4Limite;
            }

            if (salario > rentaTramo3Limite)
            {
                renta += (salario - rentaTramo3Limite) * rentaPorcentaje3;
                salario = rentaTramo3Limite;
            }

            if (salario > rentaTramo2Limite)
            {
                renta += (salario - rentaTramo2Limite) * rentaPorcentaje2;
                salario = rentaTramo2Limite;
            }

            if (salario > rentaTramo1Limite)
            {
                renta += (salario - rentaTramo1Limite) * rentaPorcentaje1;
            }

            return Math.Round(renta, 2);
        }
    }
}
