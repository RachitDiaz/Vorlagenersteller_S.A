using System.ComponentModel.DataAnnotations;

namespace API_MediSeguro.Models
{
    public class SolicitudMediSeguroMontoModel
    {
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El género es obligatorio")]
        public string Genero { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cantidad de dependientes es obligatoria")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad de dependientes no puede ser negativa")]
        public int CantidadDependientes { get; set; }
    }
}
