using Microsoft.AspNetCore.Mvc;
using Seguros.API.Data;

[ApiController]
[Route("api/[controller]")]
public class CotizacionesController : ControllerBase
{
    private readonly ICotizacionRepository _repo;
    private readonly ISegurosService _service;
    private readonly IEmailService _emailService;
    private readonly SegurosDbContext _db;

    public CotizacionesController(ICotizacionRepository repo, ISegurosService service, IEmailService emailService, SegurosDbContext db)
    {
        _repo = repo;
        _service = service;
        _emailService = emailService;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cotizaciones = await _repo.GetAllAsync();
        return Ok(cotizaciones);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c == null) return NotFound();
        var dto = new CotizacionDto
        {
            CotizacionId = c.CotizacionId,
            NumeroCotizacion = c.NumeroCotizacion,
            FechaCotizacion = c.FechaCotizacion,
            TipoSeguroId = c.TipoSeguroId,
            ClienteId = c.ClienteId,
            Moneda = c.Moneda,
            DescripcionBien = c.DescripcionBien,
            SumaAsegurada = c.SumaAsegurada,
            Tasa = c.Tasa,
            PrimaNeta = Math.Round(c.SumaAsegurada * (c.Tasa / 100m), 2)
        };
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CotizacionCreateDto dto)
    {
        var cliente = await _db.Clientes.FindAsync(dto.ClienteId);
        if (cliente == null) return BadRequest("Cliente no existe");
        var tipo = await _db.TiposSeguro.FindAsync(dto.TipoSeguroId);
        if (tipo == null) return BadRequest("Tipo de seguro no existe");

        var tasa = _service.CalculateTasa(dto.TipoSeguroId, dto.SumaAsegurada, dto.Tasa);

        var cot = new Seguros.API.Models.Cotizacion
        {
            ClienteId = dto.ClienteId,
            TipoSeguroId = dto.TipoSeguroId,
            Moneda = dto.Moneda,
            DescripcionBien = dto.DescripcionBien,
            SumaAsegurada = dto.SumaAsegurada,
            Tasa = tasa
        };

        var created = await _service.CreateCotizacionAsync(cot);

        var subject = $"Cotización {created.NumeroCotizacion}";
        var body = $@"
            <p>Hola {cliente.Nombre},</p>
            <p>Su cotización ha sido creada:</p>
            <ul>
              <li>No. Cotización: {created.NumeroCotizacion}</li>
              <li>Fecha: {created.FechaCotizacion}</li>
              <li>Tipo: {tipo.Nombre}</li>
              <li>Suma Asegurada: {created.SumaAsegurada}</li>
              <li>Tasa: {created.Tasa}%</li>
              <li>Prima Neta: {Math.Round(created.SumaAsegurada * (created.Tasa / 100m), 2)}</li>
            </ul>
        ";
        Console.WriteLine(cliente.CorreoElectronico);
        try
        {
            await _emailService.SendCotizacionEmailAsync(cliente.CorreoElectronico, subject, body);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error enviando correo: {ex.Message}");
        }

        var resultDto = new CotizacionDto
        {
            CotizacionId = created.CotizacionId,
            NumeroCotizacion = created.NumeroCotizacion,
            FechaCotizacion = created.FechaCotizacion,
            TipoSeguroId = created.TipoSeguroId,
            ClienteId = created.ClienteId,
            Moneda = created.Moneda,
            DescripcionBien = created.DescripcionBien,
            SumaAsegurada = created.SumaAsegurada,
            Tasa = created.Tasa,
            PrimaNeta = Math.Round(created.SumaAsegurada * (created.Tasa / 100m), 2)
        };

        return CreatedAtAction(nameof(Get), new { id = created.CotizacionId }, resultDto);
    }

    [HttpGet("report")]
    public async Task<IActionResult> Report([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int? tipoSeguroId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var items = await _repo.GetFilteredAsync(from, to, tipoSeguroId, page, pageSize);
        return Ok(items.Select(c => new CotizacionDto
        {
            CotizacionId = c.CotizacionId,
            NumeroCotizacion = c.NumeroCotizacion,
            FechaCotizacion = c.FechaCotizacion,
            TipoSeguroId = c.TipoSeguroId,
            ClienteId = c.ClienteId,
            Moneda = c.Moneda,
            DescripcionBien = c.DescripcionBien,
            SumaAsegurada = c.SumaAsegurada,
            Tasa = c.Tasa,
            PrimaNeta = Math.Round(c.SumaAsegurada * (c.Tasa / 100m), 2)
        }));
    }

    [HttpGet("report/export")]
    public async Task<IActionResult> Export([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int? tipoSeguroId)
    {
        var items = await _repo.GetFilteredAsync(from, to, tipoSeguroId, 1, int.MaxValue);
        var bytes = ExcelExporter.ExportCotizacionesToExcel(items);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"cotizaciones_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx");
    }
}
