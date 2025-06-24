using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace backend_planilla.Domain
{
    public class ReportePagoEmpleadoDTO
    {
        public string NombreEmpresa { get; set; }
        public string TipoContrato { get; set; }
        public string NombreEmpleado { get; set; }
        public DateTime Fecha { get; set; }
        public decimal SalarioBruto { get; set; }
        public decimal SEM { get; set; }
        public decimal IVM { get; set; }
        public decimal BPP { get; set; }
        public decimal Renta { get; set; }
        public string? BeneficioNombre1 { get; set; }
        public decimal BeneficioCosto1 { get; set; }
        public string? BeneficioNombre2 { get; set; }
        public decimal BeneficioCosto2 { get; set; }
        public string? BeneficioNombre3 { get; set; }
        public decimal BeneficioCosto3 { get; set; }
        public decimal TotalDeduccionesEmpleado { get; set; }
        public decimal TotalDeduccionesBeneficios { get; set; }
        public decimal SalarioNeto => SalarioBruto - TotalDeduccionesBeneficios - TotalDeduccionesEmpleado;
    }
}
