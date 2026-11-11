namespace DSW_3P_PR01.DTOs;

/// <summary>Representa una misión devuelta por la API.</summary>
public sealed record MisionResponse(
    int Id,
    string Nombre,
    string Dificultad,
    int RecompensaXP,
    bool Completada);
