namespace Domain.Entities;

public class OrdenTrabajo : EntidadAuditable, IEntidadConEstado
{
    public int VehiculoId { get; set; }
    public DateTime FechaIngreso { get; set; }
    public int ProblemaReportadoId { get; set; }
    public int EstadoOrdenId { get; set; }
    public int EstadoExistenciaId { get; set; }
    public Vehiculo Vehiculo { get; set; } = null!;
    public ProblemaReportado ProblemaReportado { get; set; } = null!;
    public EstadoOrden EstadoOrden { get; set; } = null!;
    public ICollection<DetalleOrdenTrabajo> Detalles { get; set; } = new List<DetalleOrdenTrabajo>();
}

public class DetalleOrdenTrabajo : EntidadAuditable, IEntidadConEstado
{
    public int OrdenTrabajoId { get; set; }
    public int ServicioId { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitarioAplicado { get; set; }
    /// <summary>Columna calculada y persistida en SQL Server: solo lectura.</summary>
    public decimal Subtotal { get; private set; }
    public int EstadoExistenciaId { get; set; }
    public OrdenTrabajo OrdenTrabajo { get; set; } = null!;
    public Servicio Servicio { get; set; } = null!;
}

/// <summary>Fila de la vista taller.vw_TotalOrdenTrabajo (sin clave).</summary>
public class TotalOrdenTrabajo
{
    public int OrdenTrabajoId { get; set; }
    public decimal Total { get; set; }
}
