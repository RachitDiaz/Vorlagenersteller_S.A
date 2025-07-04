namespace backend_planilla.Application
{
    public class DeduccionCalculada
    {
        public string NombreBeneficio { get; set; }
        public decimal MontoReducido { get; set; }
    }

    public class DeduccionCalculadaConTotal
    {
        public List<DeduccionCalculada>  DeduccionesCalculadas { get; set; }
        public decimal Total { get; set; }
    }

}
