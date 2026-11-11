using DSW_3P_PR01.DTOs;
using DSW_3P_PR01.Models;
using DSW_3P_PR01.Repositories;

namespace DSW_3P_PR01.Services;

public sealed class MisionService(IMisionRepository repository) : IMisionService
{
    public async Task<IReadOnlyList<MisionResponse>> ObtenerTodasAsync(
        CancellationToken cancellationToken)
    {
        var misiones = await repository.ObtenerTodasAsync(cancellationToken);
        return misiones.Select(CrearRespuesta).ToList();
    }

    public async Task<MisionResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken)
    {
        var mision = await repository.ObtenerPorIdAsync(id, cancellationToken);
        return mision is null ? null : CrearRespuesta(mision);
    }

    public async Task<MisionResponse> AgregarAsync(
        CreateMisionRequest request,
        CancellationToken cancellationToken)
    {
        Validar(request.Nombre, request.Dificultad, request.RecompensaXP);

        var nuevaMision = new Mision
        {
            Nombre = request.Nombre.Trim(),
            Dificultad = request.Dificultad,
            RecompensaXP = request.RecompensaXP,
            Completada = false
        };

        var creada = await repository.AgregarAsync(nuevaMision, cancellationToken);
        return CrearRespuesta(creada);
    }

    public async Task<MisionResponse?> ActualizarAsync(
        int id,
        UpdateMisionRequest request,
        CancellationToken cancellationToken)
    {
        Validar(request.Nombre, request.Dificultad, request.RecompensaXP);

        var actualizada = new Mision
        {
            Nombre = request.Nombre.Trim(),
            Dificultad = request.Dificultad,
            RecompensaXP = request.RecompensaXP,
            Completada = request.Completada
        };

        var existe = await repository.ActualizarAsync(id, actualizada, cancellationToken);
        return existe
            ? new MisionResponse(
                id,
                actualizada.Nombre,
                actualizada.Dificultad,
                actualizada.RecompensaXP,
                actualizada.Completada)
            : null;
    }

    public Task<bool> EliminarAsync(int id, CancellationToken cancellationToken)
    {
        return repository.EliminarAsync(id, cancellationToken);
    }

    private static void Validar(string nombre, string dificultad, int recompensaXP)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre de la misión es obligatorio.", nameof(nombre));
        }

        if (dificultad is not ("Fácil" or "Media" or "Difícil"))
        {
            throw new ArgumentException(
                "La dificultad debe ser Fácil, Media o Difícil.",
                nameof(dificultad));
        }

        if (recompensaXP < 0)
        {
            throw new ArgumentException(
                "La recompensa XP no puede ser negativa.",
                nameof(recompensaXP));
        }
    }

    private static MisionResponse CrearRespuesta(Mision mision)
    {
        return new MisionResponse(
            mision.Id,
            mision.Nombre,
            mision.Dificultad,
            mision.RecompensaXP,
            mision.Completada);
    }
}
