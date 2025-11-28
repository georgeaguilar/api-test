using Microsoft.EntityFrameworkCore;
using Seguros.API.Data;
using Seguros.API.Models;

public class ClienteRepository : IClienteRepository
{
    private readonly SegurosDbContext _db;
    public ClienteRepository(SegurosDbContext db) { _db = db; }

    public async Task<Cliente> AddAsync(Cliente cliente)
    {
        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();
        return cliente;
    }

    public async Task DeleteAsync(Cliente cliente)
    {
        _db.Clientes.Remove(cliente);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync() =>
        await _db.Clientes.AsNoTracking().ToListAsync();

    public async Task<Cliente?> GetByIdAsync(int id) =>
        await _db.Clientes.FindAsync(id);

    public async Task UpdateAsync(Cliente cliente)
    {
        _db.Clientes.Update(cliente);
        await _db.SaveChangesAsync();
    }
}
