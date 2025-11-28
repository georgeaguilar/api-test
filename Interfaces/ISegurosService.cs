using Seguros.API.Models;

public interface ISegurosService
{
    decimal CalculateTasa(int tipoSeguroId, decimal sumaAsegurada, decimal? providedTasa = null);
    Task<Cotizacion> CreateCotizacionAsync(Cotizacion cotizacion);
}
