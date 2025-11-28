using System.ComponentModel.DataAnnotations.Schema;

namespace Seguros.API.Models
{
    public class Cotizacion
    {
        public int CotizacionId { get; set; }

        public string NumeroCotizacion { get; set; } = null!;

        public DateTime FechaCotizacion { get; set; } = DateTime.UtcNow;

        public int TipoSeguroId { get; set; }
        public TipoSeguro? TipoSeguro { get; set; }

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public string Moneda { get; set; } = "USD";

        public string? DescripcionBien { get; set; }

        public decimal SumaAsegurada { get; set; }

        public decimal Tasa { get; set; } // porcentaje, p.ej. 2.5 => 2.5%

        [NotMapped]
        public decimal PrimaNeta => Math.Round(SumaAsegurada * (Tasa / 100m), 2);
    }
}
