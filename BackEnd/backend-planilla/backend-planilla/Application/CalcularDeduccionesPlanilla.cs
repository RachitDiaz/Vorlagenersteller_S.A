using backend_planilla.Domain;

namespace backend_planilla.Application
{
    public class CalcularDeduccionesPlanilla
    {
        public DeduccionesPlanillaModel Calcular(decimal salarioBruto)
        {
            var result = new DeduccionesPlanillaModel
            {
                SEM_Empleado = Math.Round(salarioBruto * 0.055m, 2),
                IVM_Empleado = Math.Round(salarioBruto * 0.0417m, 2),
                LPT_Empleado = Math.Round(salarioBruto * 0.01m, 2),

                SEM_Patrono = Math.Round(salarioBruto * 0.0925m, 2),
                IVM_Patrono = Math.Round(salarioBruto * 0.0508m, 2),
                LPT_Patrono = Math.Round(salarioBruto * 0.005m, 2),

                Renta = Math.Round(CalcularRenta(salarioBruto), 2)
            };

            return result;
        }

        private decimal CalcularRenta(decimal salario)
        {
            if (salario <= 941000)
                return 0;
            else if (salario <= 1382000)
                return (salario - 941000) * 0.10m;
            else if (salario <= 2421000)
                return (1382000 - 941000) * 0.10m + (salario - 1382000) * 0.15m;
            else if (salario <= 4843000)
                return (1382000 - 941000) * 0.10m +
                       (2421000 - 1382000) * 0.15m +
                       (salario - 2421000) * 0.20m;
            else
                return (1382000 - 941000) * 0.10m +
                       (2421000 - 1382000) * 0.15m +
                       (4843000 - 2421000) * 0.20m +
                       (salario - 4843000) * 0.25m;
        }
    }
}
