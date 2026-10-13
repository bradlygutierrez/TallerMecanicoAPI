using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public record DetalleOrdenTrabajoDto(int Id, int ServicioId, decimal Cantidad, decimal PrecioUnitarioAplicado, decimal Subtotal);

public record DetalleOrdenTrabajoCrearDto(
    [Range(1, int.MaxValue)] int ServicioId,
    [Range(0.01, 1e9)] decimal Cantidad,
    // Si no se envÃ­a, se usa el PrecioBase del servicio.
    [Range(0, 1e12)] decimal? PrecioUnitarioAplicado);

public record OrdenTrabajoCrearDto(
    [Range(1, int.MaxValue)] int VehiculoId,
    [Range(1, int.MaxValue)] int ProblemaReportadoId,
    [Required, MinLength(1)] List<DetalleOrdenTrabajoCrearDto> Detalles);

public record CambiarEstadoOrdenDto([Range(1, int.MaxValue)] int EstadoOrdenId);

public record OrdenTrabajoDto(
    int Id,
    int VehiculoId,
    DateTime FechaIngreso,
    int ProblemaReportadoId,
    int EstadoOrdenId,
    decimal Total,
    List<DetalleOrdenTrabajoDto> Detalles) : IConId;

