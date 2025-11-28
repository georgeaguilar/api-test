using System.ComponentModel.DataAnnotations;

namespace Seguros.API.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        [Required]
        public string Identidad { get; set; } = null!;

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public string TipoCliente { get; set; } = "Natural"; // "Natural" or "Juridico"

        public string? Telefono { get; set; }

        [Required]
        public string CorreoElectronico { get; set; } = null!;

        public ICollection<Cotizacion>? Cotizaciones { get; set; }
    }
}
