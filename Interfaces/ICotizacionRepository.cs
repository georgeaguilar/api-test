using Seguros.API.Models;

public interface ICotizacionRepository
{
    Task<Cotizacion> AddAsync(Cotizacion cotizacion);
    Task<Cotizacion?> GetByIdAsync(int id);
    Task<IEnumerable<Cotizacion>> GetAllAsync();
    Task<IEnumerable<Cotizacion>> GetFilteredAsync(DateTime? from, DateTime? to, int? tipoSeguroId, int page = 1, int pageSize = 50);
    Task<int> CountFilteredAsync(DateTime? from, DateTime? to, int? tipoSeguroId);
}
