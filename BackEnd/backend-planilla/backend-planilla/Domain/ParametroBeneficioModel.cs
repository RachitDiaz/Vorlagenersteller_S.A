namespace backend_planilla.Domain
{
    public class ParametroBeneficioModel
    {
        public int IDParametro { get; set; }
        public int IDBeneficio { get; set; }
        public string Nombre { get; set; }
        public string TipoParametro { get; set; }
        public string DatoIngreso { get; set; }
        public int ValorParametro { get; set; }
    }
}
