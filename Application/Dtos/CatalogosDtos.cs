using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public record MarcaDto(int Id, string Nombre, string? PaisOrigen) : IConId;
public record MarcaSaveDto(
    [Required, StringLength(100)] string Nombre,
    [StringLength(100)] string? PaisOrigen);

public record ModeloDto(int Id, int MarcaId, string Nombre, string? Descripcion) : IConId;
public record ModeloSaveDto(
    [Range(1, int.MaxValue)] int MarcaId,
    [Required, StringLength(100)] string Nombre,
    [StringLength(250)] string? Descripcion);

public record ServicioDto(int Id, string Nombre, string? Descripcion, decimal PrecioBase) : IConId;
public record ServicioSaveDto(
    [Required, StringLength(120)] string Nombre,
    [StringLength(500)] string? Descripcion,
    [Range(0, 1e12)] decimal PrecioBase);

public record EstadoOrdenDto(int Id, string Nombre, string? Descripcion) : IConId;
public record EstadoOrdenSaveDto(
    [Required, StringLength(60)] string Nombre,
    [StringLength(250)] string? Descripcion);

public record ProblemaReportadoDto(int Id, string Nombre, string? Descripcion) : IConId;
public record ProblemaReportadoSaveDto(
    [Required, StringLength(120)] string Nombre,
    [StringLength(500)] string? Descripcion);

