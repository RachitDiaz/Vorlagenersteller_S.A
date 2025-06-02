namespace backend_planilla.Domain
{
    public class ParametroBeneficioModel
    {
        public int IDParametro { get; set; }
        public int IDBeneficio { get; set; }
        public string Nombre { get; set; }
        public string TipoDeDatoParametro { get; set; }
        public string TipoValorParametro { get; set; }
        public int ValorDelParametro { get; set; }
    }
}
