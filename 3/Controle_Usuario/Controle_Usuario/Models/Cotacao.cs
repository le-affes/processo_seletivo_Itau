namespace ControleUsuario.Models;

public class Cotacao
{
    public int Id { get; set; }
    public int IdAtivo { get; set; }
    public decimal PrcoUni { get; set; }
    public DateTime DtHora { get; set; }
}