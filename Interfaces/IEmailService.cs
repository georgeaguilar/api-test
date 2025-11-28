public interface IEmailService
{
    Task SendCotizacionEmailAsync(string toEmail, string subject, string body);
}
