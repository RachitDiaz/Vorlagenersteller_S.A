namespace backend_planilla.Domain
{
    public class PlanillaMensualEmpleadoModel
    {
        public int IdPlanilla { get; set; }
        public string CedulaEmpleado { get; set; } = string.Empty;

        public decimal SalarioBruto { get; set; }

        public decimal SEMEmpleado { get; set; }
        public decimal SEMPatrono { get; set; }

        public decimal IVMEmpleado { get; set; }
        public decimal IVMPatrono { get; set; }

        public decimal LPTEmpleado { get; set; }
        public decimal LPTPatrono { get; set; }

        public decimal ImpuestoRenta { get; set; }

        public decimal? Beneficio1 { get; set; }
        public decimal? Beneficio2 { get; set; }
        public decimal? Beneficio3 { get; set; }

        public decimal TotalDeduccionesEmpleado { get; set; }
        public decimal TotalDeduccionesPatrono { get; set; }

        public DateTime FechaInicioTrabajo { get; set; }

        public int TiempoLaboradoDias
        {
            get
            {
                var dias = (DateTime.Now - FechaInicioTrabajo).Days;
                return dias > 30 ? 30 : dias;
            }
        }
    }
}
