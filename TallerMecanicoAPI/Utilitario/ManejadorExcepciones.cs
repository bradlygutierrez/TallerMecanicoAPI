using Application.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TallerMecanicoAPI.Utilitario;

public class ManejadorExcepciones(ILogger<ManejadorExcepciones> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext contexto, Exception excepcion, CancellationToken ct)
    {
        var (estado, titulo) = excepcion switch
        {
            NegocioException => (StatusCodes.Status400BadRequest, excepcion.Message),
            DbUpdateException => (StatusCodes.Status409Conflict,
                "No se pudo guardar: el registro duplica un valor único o referencia datos inexistentes."),
            _ => (StatusCodes.Status500InternalServerError, "Ocurrió un error interno.")
        };

        if (estado == StatusCodes.Status500InternalServerError)
            logger.LogError(excepcion, "Error no controlado");
        else
            logger.LogWarning(excepcion, "Solicitud rechazada: {Mensaje}", excepcion.Message);

        contexto.Response.StatusCode = estado;
        await contexto.Response.WriteAsJsonAsync(new ProblemDetails { Status = estado, Title = titulo }, ct);
        return true;
    }
}
