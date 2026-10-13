using Application.Services;
using Microsoft.AspNetCore.Identity;

namespace TallerMecanicoAPI.Utilitario;

/// <summary>Usa el PasswordHasher de ASP.NET Core (PBKDF2 con sal) sin necesitar tablas de Identity.</summary>
public class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();
    private static readonly object Contexto = new();

    public string Hashear(string password) => _hasher.HashPassword(Contexto, password);

    public bool Verificar(string hash, string password) =>
        _hasher.VerifyHashedPassword(Contexto, hash, password) != PasswordVerificationResult.Failed;
}
