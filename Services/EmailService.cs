using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;

public class SmtpSettings
{
    public string Host { get; set; } = null!;
    public int Port { get; set; } = 587;
    public bool UseSsl { get; set; } = true;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FromName { get; set; } = "Seguros";
    public string FromEmail { get; set; } = null!;
}

public class EmailService : IEmailService
{
    private readonly SmtpSettings _settings;
    public EmailService(IOptions<SmtpSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendCotizacionEmailAsync(string toEmail, string subject, string body)
    {
        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        msg.To.Add(MailboxAddress.Parse(toEmail));
        msg.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = body, TextBody = body };
        msg.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.Host, _settings.Port, _settings.UseSsl);
        if (!string.IsNullOrEmpty(_settings.UserName))
        {
            await client.AuthenticateAsync(_settings.UserName, _settings.Password);
        }
        await client.SendAsync(msg);
        await client.DisconnectAsync(true);
    }
}
