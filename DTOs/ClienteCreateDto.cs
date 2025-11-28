public class ClienteCreateDto
{
    public string Nombre { get; set; } = null!;
    public string Identidad { get; set; } = null!;
    public DateTime FechaNacimiento { get; set; }
    public string TipoCliente { get; set; } = "Natural";
    public string? Telefono { get; set; }
    public string CorreoElectronico { get; set; } = null!;
}
