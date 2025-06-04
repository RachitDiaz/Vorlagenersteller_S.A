namespace backend_planilla.Domain
{
    public class DeduccionesPlanillaModel
    {
        public decimal SEM_Empleado { get; set; }
        public decimal SEM_Patrono { get; set; }

        public decimal IVM_Empleado { get; set; }
        public decimal IVM_Patrono { get; set; }

        public decimal LPT_Empleado { get; set; }
        public decimal LPT_Patrono { get; set; }

        public decimal Renta { get; set; }

        public decimal TotalEmpleado => SEM_Empleado + IVM_Empleado + LPT_Empleado + Renta;
        public decimal TotalPatrono => SEM_Patrono + IVM_Patrono + LPT_Patrono;
    }
}
