namespace Domain.Entities;

public class Cliente : EntidadAuditable, IEntidadConEstado
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? CorreoElectronico { get; set; }
    public int EstadoExistenciaId { get; set; }
    public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}

public class Vehiculo : EntidadAuditable, IEntidadConEstado
{
    public string Placa { get; set; } = string.Empty;
    public int MarcaId { get; set; }
    public int ModeloId { get; set; }
    public short AnioFabricacion { get; set; }
    public int ClienteId { get; set; }
    public int EstadoExistenciaId { get; set; }
    public Cliente Cliente { get; set; } = null!;
    public Marca Marca { get; set; } = null!;
    public Modelo Modelo { get; set; } = null!;
}
