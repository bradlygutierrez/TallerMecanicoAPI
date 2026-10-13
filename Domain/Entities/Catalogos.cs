namespace Domain.Entities;

public class Marca : EntidadAuditable, IEntidadConEstado
{
    public string Nombre { get; set; } = string.Empty;
    public string? PaisOrigen { get; set; }
    public int EstadoExistenciaId { get; set; }
    public ICollection<Modelo> Modelos { get; set; } = new List<Modelo>();
}

public class Modelo : EntidadAuditable, IEntidadConEstado
{
    public int MarcaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int EstadoExistenciaId { get; set; }
    public Marca Marca { get; set; } = null!;
    public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}

public class Servicio : EntidadAuditable, IEntidadConEstado
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal PrecioBase { get; set; }
    public int EstadoExistenciaId { get; set; }
}

public class EstadoOrden : EntidadAuditable
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}

public class ProblemaReportado : EntidadAuditable
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
