namespace Application.Services;

/// <summary>Error de regla de negocio: la API lo devuelve como 400.</summary>
public class NegocioException(string mensaje) : Exception(mensaje);
