namespace DSW_3P_PR01.Models;

public sealed class Mision
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Dificultad { get; set; } = string.Empty;
    public int RecompensaXP { get; set; }
    public bool Completada { get; set; }
}