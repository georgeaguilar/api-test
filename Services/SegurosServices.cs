using Seguros.API.Data;
using Seguros.API.Models;

public class SegurosService : ISegurosService
{
    private readonly ICotizacionRepository _cotRepo;
    private readonly SegurosDbContext _db;
    public SegurosService(ICotizacionRepository cotRepo, SegurosDbContext db)
    {
        _cotRepo = cotRepo;
        _db = db;
    }

    public decimal CalculateTasa(int tipoSeguroId, decimal sumaAsegurada, decimal? providedTasa = null)
    {
        if (providedTasa.HasValue) return providedTasa.Value;

        switch (tipoSeguroId)
        {
            case 2:
                if (sumaAsegurada < 100_000m) return 1.5m;
                if (sumaAsegurada <= 500_000m) return 2.0m;
                return 2.5m;
            case 1:
                if (sumaAsegurada < 50_000m) return 0.8m;
                return 1.2m;
            case 3:
                return 1.0m;
            case 4:
                return 3.0m;
            default:
                return 1.0m;
        }
    }

    public async Task<Cotizacion> CreateCotizacionAsync(Cotizacion cotizacion)
    {
        cotizacion.NumeroCotizacion = $"COT-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
        cotizacion.FechaCotizacion = DateTime.UtcNow;

        var added = await _cotRepo.AddAsync(cotizacion);
        return added;
    }
}
