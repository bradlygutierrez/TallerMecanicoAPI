using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public record LoginDto(
    [Required] string NombreUsuario,
    [Required] string Password);

public record LoginRespuestaDto(string Token, DateTime Expira, int UsuarioId, string NombreUsuario, string Rol);

public record RegistrarUsuarioDto(
    [Required, StringLength(80)] string NombreUsuario,
    [Required, StringLength(100, MinimumLength = 8)] string Password,
    [Range(1, int.MaxValue)] int RolId,
    [StringLength(25)] string? Telefono);

public record UsuarioDto(int Id, string NombreUsuario, string Rol, string? Telefono);

