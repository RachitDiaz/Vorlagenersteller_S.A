namespace backend_planilla.Domain
{
    public class PlanillaEmpresaModel
    {
        public int IdPlanilla { get; set; }
        public string CedulaEmpresa { get; set; } = string.Empty;
        public string PeriodoMes { get; set; } = string.Empty;

        public decimal TotalSEMPagar { get; set; }
        public decimal TotalSEMDeducir { get; set; }

        public decimal TotalIVMPagar { get; set; }
        public decimal TotalIVMDeducir { get; set; }

        public decimal TotalLPTPagar { get; set; }
        public decimal TotalLPTDeducir { get; set; }

        public decimal TotalRentaDeducir { get; set; }
        public decimal TotalBeneficiosDeducir { get; set; }

        public DateTime FechaGeneracion { get; set; } = DateTime.Now;
    }
}
