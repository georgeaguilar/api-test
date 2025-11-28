using Seguros.API.Models;

public interface IClienteRepository
{
    Task<Cliente> AddAsync(Cliente cliente);
    Task<Cliente?> GetByIdAsync(int id);
    Task<IEnumerable<Cliente>> GetAllAsync();
    Task UpdateAsync(Cliente cliente);
    Task DeleteAsync(Cliente cliente);
}
