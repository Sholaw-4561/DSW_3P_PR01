using DSW_3P_PR01.Models;

namespace DSW_3P_PR01.Repositories;

public interface IMisionRepository
{
    Task<IReadOnlyList<Mision>> ObtenerTodasAsync(CancellationToken cancellationToken);
    Task<Mision?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);
    Task<Mision> AgregarAsync(Mision nuevaMision, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(int id, Mision mision, CancellationToken cancellationToken);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken);
}
