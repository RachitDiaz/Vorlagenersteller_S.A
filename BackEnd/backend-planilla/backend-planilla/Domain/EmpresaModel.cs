namespace backend_planilla.Domain
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

    public class CorreoModel
    {
        public string cedulaJuridica { get; set; }
        public string correo { get; set; }
    }

    public class TelefonoModel
    {
        public string cedulaJuridica { get; set; }
        public string telefono { get; set; }
    }

    public class DireccionModel
    {
        public string cedulaJuridica { get; set; }
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string distrito { get; set; }
        public string otrasSenas { get; set; }
    }

    public class AgregarEmpresaModel
    {
        public EmpresaModel empresa { get; set; }
        public CorreoModel correo { get; set; }
        public TelefonoModel telefono { get; set; }
        public DireccionModel direccion { get; set; }
    }
}
