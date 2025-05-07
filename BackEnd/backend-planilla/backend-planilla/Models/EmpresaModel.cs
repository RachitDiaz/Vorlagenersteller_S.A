namespace backend_planilla.Models
{
    public class EmpresaModel
    {
        public string CedulaJuridica { get; set; }
        public string CedulaDueno { get; set; }
        public string CedulaAdmin { get; set; }
        public string TipoDePago { get; set; }
        public string RazonSocial { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int BeneficiosMaximos { get; set; }
        public DateTime FechaDeModificacion { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        public int UsuarioCreador { get; set; }
        public int UltimoEnModificar { get; set; }
        public bool Activo { get; set; }

    }
 }
