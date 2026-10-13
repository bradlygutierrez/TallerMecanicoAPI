using System.Security.Claims;
using System.Text;
using Application.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace TallerMecanicoAPI.Utilitario;

public class JwtTokenService(IOptions<JwtOpciones> opciones) : ITokenService
{
    public (string Token, DateTime Expira) Generar(int usuarioId, string nombreUsuario, string rol)
    {
        var o = opciones.Value;
        var expira = DateTime.UtcNow.AddMinutes(o.ExpiraMinutos);
        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(o.Key));

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("sub", usuarioId.ToString()),
                new Claim("name", nombreUsuario),
                new Claim("role", rol)
            }),
            Expires = expira,
            Issuer = o.Issuer,
            Audience = o.Audience,
            SigningCredentials = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256)
        };

        return (new JsonWebTokenHandler().CreateToken(descriptor), expira);
    }
}
