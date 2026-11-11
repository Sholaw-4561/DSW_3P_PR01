using System.Text.Json;
using DSW_3P_PR01.Models;

namespace DSW_3P_PR01.Repositories;

public sealed class MisionJsonRepository : IMisionRepository
{
    private static readonly SemaphoreSlim FileLock = new(1, 1);
    private readonly string _ruta;
    private readonly JsonSerializerOptions _opciones = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public MisionJsonRepository(IWebHostEnvironment environment)
    {
        var directorioDatos = Path.Combine(environment.ContentRootPath, "data");
        Directory.CreateDirectory(directorioDatos);

        _ruta = Path.Combine(directorioDatos, "misiones.json");
        if (!File.Exists(_ruta))
        {
            File.WriteAllText(_ruta, "[]");
        }
    }

    public Task<IReadOnlyList<Mision>> ObtenerTodasAsync(CancellationToken cancellationToken)
    {
        return ConBloqueoAsync<IReadOnlyList<Mision>>(
            async token => await LeerAsync(token),
            cancellationToken);
    }

    public Task<Mision?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken)
    {
        return ConBloqueoAsync(
            async token => (await LeerAsync(token)).FirstOrDefault(mision => mision.Id == id),
            cancellationToken);
    }

    public Task<Mision> AgregarAsync(Mision nuevaMision, CancellationToken cancellationToken)
    {
        return ConBloqueoAsync(async token =>
        {
            var misiones = await LeerAsync(token);
            nuevaMision.Id = misiones.Count == 0 ? 1 : misiones.Max(mision => mision.Id) + 1;
            misiones.Add(nuevaMision);
            await GuardarAsync(misiones, token);
            return nuevaMision;
        }, cancellationToken);
    }

    public Task<bool> ActualizarAsync(int id, Mision mision, CancellationToken cancellationToken)
    {
        return ConBloqueoAsync(async token =>
        {
            var misiones = await LeerAsync(token);
            var existente = misiones.FirstOrDefault(item => item.Id == id);
            if (existente is null)
            {
                return false;
            }

            existente.Nombre = mision.Nombre;
            existente.Dificultad = mision.Dificultad;
            existente.RecompensaXP = mision.RecompensaXP;
            existente.Completada = mision.Completada;
            await GuardarAsync(misiones, token);
            return true;
        }, cancellationToken);
    }

    public Task<bool> EliminarAsync(int id, CancellationToken cancellationToken)
    {
        return ConBloqueoAsync(async token =>
        {
            var misiones = await LeerAsync(token);
            var existente = misiones.FirstOrDefault(item => item.Id == id);
            if (existente is null)
            {
                return false;
            }

            misiones.Remove(existente);
            await GuardarAsync(misiones, token);
            return true;
        }, cancellationToken);
    }

    private async Task<T> ConBloqueoAsync<T>(
        Func<CancellationToken, Task<T>> operacion,
        CancellationToken cancellationToken)
    {
        await FileLock.WaitAsync(cancellationToken);
        try
        {
            return await operacion(cancellationToken);
        }
        finally
        {
            FileLock.Release();
        }
    }

    private async Task<List<Mision>> LeerAsync(CancellationToken cancellationToken)
    {
        var json = await File.ReadAllTextAsync(_ruta, cancellationToken);
        return JsonSerializer.Deserialize<List<Mision>>(json, _opciones) ?? [];
    }

    private async Task GuardarAsync(List<Mision> misiones, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(misiones, _opciones);
        var rutaTemporal = $"{_ruta}.tmp";

        await File.WriteAllTextAsync(rutaTemporal, json, cancellationToken);
        File.Move(rutaTemporal, _ruta, overwrite: true);
    }
}
