using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend_planilla.Models
{
    public class EmpleadoModel
    {
        public string CedulaEmpleado { get; set; }
        public string CedulaEmpresa { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Banco { get; set; }
        public decimal SalarioBruto { get; set; }
        public string TipoContrato { get; set; }
    }
}
