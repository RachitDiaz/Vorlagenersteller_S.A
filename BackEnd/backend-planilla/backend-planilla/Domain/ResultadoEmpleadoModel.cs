namespace backend_planilla.Domain
{
    public class ResultadoEmpleadoModel
    {
        public string CedulaEmpleado { get; set; } = string.Empty;
        public decimal SalarioBruto { get; set; }

        public DateTime FechaGeneracion { get; set; }
        public DeduccionesObligatoriasModel Deducciones { get; set; }
    }
}
