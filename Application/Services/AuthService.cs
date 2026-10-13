using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

/// <summary>Implementado en la API (Utilitario): hash de contraseñas.</summary>
public interface IPasswordHasher
{
    string Hashear(string password);
    bool Verificar(string hash, string password);
}

/// <summary>Implementado en la API (Utilitario): emisión del JWT.</summary>
public interface ITokenService
{
    (string Token, DateTime Expira) Generar(int usuarioId, string nombreUsuario, string rol);
}

public interface IAuthService
{
    Task<LoginRespuestaDto?> LoginAsync(LoginDto dto, CancellationToken ct = default);
    Task<UsuarioDto> RegistrarAsync(RegistrarUsuarioDto dto, CancellationToken ct = default);
}

public class AuthService(
    IUsuarioRepository usuarios,
    IRepository<Rol> roles,
    IPasswordHasher hasher,
    ITokenService tokens,
    IUnitOfWork unidadTrabajo,
    IMapper mapper) : IAuthService
{
    public async Task<LoginRespuestaDto?> LoginAsync(LoginDto dto, CancellationToken ct = default)
    {
        var usuario = await usuarios.ObtenerPorNombreAsync(dto.NombreUsuario, ct);

        // El usuario 'sistema' no tiene PasswordHash, por lo que nunca puede iniciar sesión.
        if (usuario is null || string.IsNullOrEmpty(usuario.PasswordHash) ||
            !hasher.Verificar(usuario.PasswordHash, dto.Password))
            return null;

        var (token, expira) = tokens.Generar(usuario.Id, usuario.NombreUsuario, usuario.Rol.Nombre);
        return new LoginRespuestaDto(token, expira, usuario.Id, usuario.NombreUsuario, usuario.Rol.Nombre);
    }

    public async Task<UsuarioDto> RegistrarAsync(RegistrarUsuarioDto dto, CancellationToken ct = default)
    {
        if (await usuarios.ExisteNombreAsync(dto.NombreUsuario, ct))
            throw new NegocioException("El nombre de usuario ya está en uso.");

        var rol = await roles.ObtenerPorIdAsync(dto.RolId, ct)
            ?? throw new NegocioException("El rol no existe.");

        var usuario = new Usuario
        {
            RolId = rol.Id,
            Rol = rol,
            NombreUsuario = dto.NombreUsuario,
            Telefono = dto.Telefono,
            PasswordHash = hasher.Hashear(dto.Password),
            EstadoExistenciaId = EstadoExistenciaIds.Activo
        };

        await usuarios.AgregarAsync(usuario, ct);
        await unidadTrabajo.SaveChangesAsync(ct);
        return mapper.Map<UsuarioDto>(usuario);
    }
}
