namespace backend_planilla.Domain
{
    public class DeduccionBeneficioModel
    {
        public int IDBeneficio { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; } 
        public decimal Monto { get; set; } 

        public string Descripción { get; set; }
    }
}
