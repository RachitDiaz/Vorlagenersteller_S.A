namespace backend_planilla.Domain
{
    public class GenerarPlanillaRequestModel
    {
        public string CedulaJuridica { get; set; } = string.Empty;
        public string TipoPlanilla { get; set; } = "mensual"; // o "quincenal"
        public DateTime FechaGeneracion { get; set; }
        public string Periodo { get; set; } = string.Empty;
    }
}
