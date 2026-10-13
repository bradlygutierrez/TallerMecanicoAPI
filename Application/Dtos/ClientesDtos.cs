using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public record ClienteDto(int Id, string NombreCompleto, string? Telefono, string? CorreoElectronico) : IConId;
public record ClienteSaveDto(
    [Required, StringLength(150)] string NombreCompleto,
    [StringLength(25)] string? Telefono,
    [EmailAddress, StringLength(254)] string? CorreoElectronico);

public record VehiculoDto(int Id, string Placa, int MarcaId, int ModeloId, short AnioFabricacion, int ClienteId) : IConId;
public record VehiculoSaveDto(
    [Required, StringLength(20)] string Placa,
    [Range(1, int.MaxValue)] int MarcaId,
    [Range(1, int.MaxValue)] int ModeloId,
    [Range(1886, 2100)] short AnioFabricacion,
    [Range(1, int.MaxValue)] int ClienteId);

