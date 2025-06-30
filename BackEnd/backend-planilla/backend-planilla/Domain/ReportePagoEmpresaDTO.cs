using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend_planilla.Domain
{
    public class ReportePagoEmpresaDTO
    {
        public string NombreEmpresa { get; set; }
        public string NombreEmpleador { get; set; }
        public string FechaPago { get; set; }
        public string PeridoPago { get; set; }
        public decimal SalariosTiempoCompleto { get; set; }
        public decimal TotalSEM { get; set; }
        public decimal TotalIVM { get; set; }
        public decimal TotalBP { get; set; }
        public decimal TotalAF { get; set; }
        public decimal TotalIMAS { get; set; }
        public decimal TotalINA { get; set; }
        public decimal TotalFCL { get; set; }
        public decimal TotalPC { get; set; }
        public decimal TotalINS { get; set; }
        public decimal TotalPagosLey => TotalSEM + TotalIVM + TotalBP + TotalAF + TotalIMAS + TotalINA + TotalFCL + TotalPC + TotalINS; 
        public decimal BeneficiosTotales { get; set; }
        public decimal CostoTotal => BeneficiosTotales + TotalPagosLey + SalariosTiempoCompleto;
    }
}
