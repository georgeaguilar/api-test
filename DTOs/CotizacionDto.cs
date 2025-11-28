public class CotizacionDto
{
    public int CotizacionId { get; set; }
    public string NumeroCotizacion { get; set; } = null!;
    public DateTime FechaCotizacion { get; set; }
    public int TipoSeguroId { get; set; }
    public int ClienteId { get; set; }
    public string Moneda { get; set; } = "USD";
    public string? DescripcionBien { get; set; }
    public decimal SumaAsegurada { get; set; }
    public decimal Tasa { get; set; }
    public decimal PrimaNeta { get; set; }
}
