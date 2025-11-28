using System.Text.Json.Serialization;

namespace Seguros.API.Models
{
    public class TipoSeguro
    {
        public int TipoSeguroId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }

        [JsonIgnore]
        public ICollection<Cotizacion>? Cotizaciones { get; set; }
    }
}
