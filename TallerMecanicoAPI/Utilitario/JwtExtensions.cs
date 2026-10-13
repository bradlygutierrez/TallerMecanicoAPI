using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TallerMecanicoAPI.Utilitario;

public static class JwtExtensions
{
    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuracion)
    {
        var jwt = configuracion.GetSection("Jwt").Get<JwtOpciones>()
            ?? throw new InvalidOperationException("Falta la sección 'Jwt' en la configuración.");
        if (jwt.Key.Length < 32)
            throw new InvalidOperationException("Jwt:Key debe tener al menos 32 caracteres.");

        services.Configure<JwtOpciones>(configuracion.GetSection("Jwt"));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opciones =>
            {
                opciones.MapInboundClaims = false; // conservamos "sub", "name" y "role" tal cual
                opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1),
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });
        services.AddAuthorization();
        return services;
    }
}
