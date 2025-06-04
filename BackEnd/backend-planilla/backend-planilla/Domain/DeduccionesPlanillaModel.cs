namespace backend_planilla.Domain
{
    public class DeduccionesPlanillaModel
    {
        public decimal SEMEmpleado { get; set; }
        public decimal SEMPatrono { get; set; }

        public decimal IVMEmpleado { get; set; }
        public decimal IVMPatrono { get; set; }

        public decimal LPTEmpleado { get; set; }
        public decimal LPTPatrono { get; set; }

        public decimal ImpuestoRenta { get; set; }

        public decimal TotalEmpleado => SEMEmpleado + IVMEmpleado + LPTEmpleado + ImpuestoRenta;
        public decimal TotalPatrono => SEMPatrono + IVMPatrono + LPTPatrono;
    }
}
