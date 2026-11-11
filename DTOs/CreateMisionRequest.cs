using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR01.DTOs;

/// <summary>Datos necesarios para registrar una misión.</summary>
public sealed record CreateMisionRequest
{
    [Required]
    public required string Nombre { get; init; }

    [Required]
    [RegularExpression("^(Fácil|Media|Difícil)$")]
    public required string Dificultad { get; init; }

    [Range(0, int.MaxValue)]
    public int RecompensaXP { get; init; }
}
