namespace backend_planilla.Domain
{
    public class DeduccionesObligatoriasModel
    {
        public decimal SEMEmpleado { get; set; }
        public decimal IVMEmpleado { get; set; }
        public decimal BPPOEmpleado { get; set; }
        public decimal ImpuestoRenta { get; set; }

        public decimal SEMPatrono { get; set; }
        public decimal IVMPatrono { get; set; }
        public decimal BPOPPatrono { get; set; }
        public decimal AsignacionesFamiliaresPatrono { get; set; }
        public decimal IMASPatrono { get; set; }
        public decimal INAPatrono { get; set; }
        public decimal FCLPatrono { get; set; }
        public decimal OPCPatrono { get; set; }
        public decimal INSPatrono { get; set; }

        public decimal TotalEmpleado => SEMEmpleado + IVMEmpleado + BPPOEmpleado + ImpuestoRenta;
        public decimal TotalPatrono => SEMPatrono + IVMPatrono + BPOPPatrono + AsignacionesFamiliaresPatrono + IMASPatrono + INAPatrono + FCLPatrono + OPCPatrono + INSPatrono;
    }
}


