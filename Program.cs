using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Seguros.API.Data;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContext<SegurosDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ICotizacionRepository, CotizacionRepository>();
builder.Services.AddScoped<ISegurosService, SegurosService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.Configure<SmtpSettings>(options =>
{
    options.Host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "smtp.gmail.com";
    options.Port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "465");
    options.UseSsl = bool.Parse(Environment.GetEnvironmentVariable("SMTP_USESSL") ?? "true");
    options.UserName = Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? "";
    options.Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "";
    options.FromName = Environment.GetEnvironmentVariable("SMTP_FROMNAME") ?? "Seguros";
    options.FromEmail = Environment.GetEnvironmentVariable("SMTP_FROMEMAIL") ?? "no-reply@seguros.local";
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Seguros API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
