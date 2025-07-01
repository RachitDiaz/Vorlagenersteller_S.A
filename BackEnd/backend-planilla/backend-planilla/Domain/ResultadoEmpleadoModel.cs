using backend_planilla.Application;

namespace backend_planilla.Domain
{
    public class ResultadoEmpleadoModel
    {
        public string CedulaEmpleado { get; set; } = string.Empty;
        public decimal SalarioBruto { get; set; }

        public required DeduccionesObligatoriasModel Deducciones { get; set; }

        public required DeduccionCalculadaConTotal Beneficios { get; set; }
    }
}
