public class CotizacionCreateDto
{
    public int TipoSeguroId { get; set; }
    public int ClienteId { get; set; }
    public string Moneda { get; set; } = "USD";
    public string? DescripcionBien { get; set; }
    public decimal SumaAsegurada { get; set; }
    public decimal? Tasa { get; set; } // si null, servicio decidirá la tasa por tipo/suma
}
