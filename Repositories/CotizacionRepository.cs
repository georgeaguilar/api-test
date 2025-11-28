using Microsoft.EntityFrameworkCore;
using Seguros.API.Data;
using Seguros.API.Models;

public class CotizacionRepository : ICotizacionRepository
{
    private readonly SegurosDbContext _db;
    public CotizacionRepository(SegurosDbContext db) { _db = db; }

    public async Task<Cotizacion> AddAsync(Cotizacion cotizacion)
    {
        _db.Cotizaciones.Add(cotizacion);
        await _db.SaveChangesAsync();
        return cotizacion;
    }

    public async Task<int> CountFilteredAsync(DateTime? from, DateTime? to, int? tipoSeguroId)
    {
        var q = _db.Cotizaciones.AsQueryable();
        if (from.HasValue) q = q.Where(c => c.FechaCotizacion >= from.Value);
        if (to.HasValue) q = q.Where(c => c.FechaCotizacion <= to.Value);
        if (tipoSeguroId.HasValue) q = q.Where(c => c.TipoSeguroId == tipoSeguroId.Value);
        return await q.CountAsync();
    }

    public async Task<IEnumerable<Cotizacion>> GetAllAsync() =>
        await _db.Cotizaciones.Include(c => c.Cliente).Include(c => c.TipoSeguro).AsNoTracking().ToListAsync();

    public async Task<Cotizacion?> GetByIdAsync(int id) =>
        await _db.Cotizaciones.Include(c => c.Cliente).Include(c => c.TipoSeguro).FirstOrDefaultAsync(c => c.CotizacionId == id);

    public async Task<IEnumerable<Cotizacion>> GetFilteredAsync(DateTime? from, DateTime? to, int? tipoSeguroId, int page = 1, int pageSize = 50)
    {
        var q = _db.Cotizaciones.Include(c => c.Cliente).Include(c => c.TipoSeguro).AsQueryable();
        if (from.HasValue) q = q.Where(c => c.FechaCotizacion >= from.Value);
        if (to.HasValue) q = q.Where(c => c.FechaCotizacion <= to.Value);
        if (tipoSeguroId.HasValue) q = q.Where(c => c.TipoSeguroId == tipoSeguroId.Value);
        q = q.OrderByDescending(c => c.FechaCotizacion)
             .Skip((page - 1) * pageSize)
             .Take(pageSize);
        return await q.AsNoTracking().ToListAsync();
    }
}
