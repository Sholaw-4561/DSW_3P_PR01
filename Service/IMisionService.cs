using DSW_3P_PR01.DTOs;

namespace DSW_3P_PR01.Services;

public interface IMisionService
{
    Task<IReadOnlyList<MisionResponse>> ObtenerTodasAsync(CancellationToken cancellationToken);
    Task<MisionResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);
    Task<MisionResponse> AgregarAsync(CreateMisionRequest request, CancellationToken cancellationToken);
    Task<MisionResponse?> ActualizarAsync(
        int id,
        UpdateMisionRequest request,
        CancellationToken cancellationToken);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken);
}
