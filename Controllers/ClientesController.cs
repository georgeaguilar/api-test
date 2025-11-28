using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteRepository _repo;
    public ClientesController(IClienteRepository repo) { _repo = repo; }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _repo.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c == null) return NotFound();
        return Ok(c);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClienteCreateDto dto)
    {
        var model = new Seguros.API.Models.Cliente
        {
            Nombre = dto.Nombre,
            Identidad = dto.Identidad,
            FechaNacimiento = dto.FechaNacimiento,
            TipoCliente = dto.TipoCliente,
            Telefono = dto.Telefono,
            CorreoElectronico = dto.CorreoElectronico
        };
        var added = await _repo.AddAsync(model);
        return CreatedAtAction(nameof(Get), new { id = added.ClienteId }, added);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return NotFound();

        existing.Nombre = dto.Nombre;
        existing.Identidad = dto.Identidad;
        existing.FechaNacimiento = dto.FechaNacimiento;
        existing.TipoCliente = dto.TipoCliente;
        existing.Telefono = dto.Telefono;
        existing.CorreoElectronico = dto.CorreoElectronico;

        await _repo.UpdateAsync(existing);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return NotFound();
        await _repo.DeleteAsync(existing);
        return NoContent();
    }
}
