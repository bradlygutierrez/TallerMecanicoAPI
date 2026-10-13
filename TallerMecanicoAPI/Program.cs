using Application.Services;
using Domain.Interfaces;
using Infraestructure.IOC;
using TallerMecanicoAPI.Utilitario;

var builder = WebApplication.CreateBuilder(args);

var cadenaConexion = builder.Configuration.GetConnectionString("TallerDb")
    ?? throw new InvalidOperationException("Falta la cadena de conexión 'TallerDb'.");

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();

// Capas
builder.Services.AddApplication();
builder.Services.AddInfraestructure(cadenaConexion);

// Servicios propios de la API (implementan interfaces de Domain/Application)
builder.Services.AddScoped<IUsuarioActual, UsuarioActual>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasherService>();
builder.Services.AddSingleton<ITokenService, JwtTokenService>();

builder.Services.AddJwt(builder.Configuration);
builder.Services.AddExceptionHandler<ManejadorExcepciones>();
builder.Services.AddProblemDetails();

var app = builder.Build();

await DbInicializador.CrearAdministradorAsync(app.Services, app.Configuration);

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
