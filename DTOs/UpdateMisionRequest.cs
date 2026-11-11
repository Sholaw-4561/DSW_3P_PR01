using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR01.DTOs;

/// <summary>Datos para reemplazar la información de una misión existente.</summary>
public sealed record UpdateMisionRequest
{
    [Required]
    public required string Nombre { get; init; }

    [Required]
    [RegularExpression("^(Fácil|Media|Difícil)$")]
    public required string Dificultad { get; init; }

    [Range(0, int.MaxValue)]
    public int RecompensaXP { get; init; }

    public bool Completada { get; init; }
}
