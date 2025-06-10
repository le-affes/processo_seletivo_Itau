namespace WorkerService.Domain;

public class Operacao
{
    public int Id { get; set; }
    public int IdUsuario { get; set; }
    public int IdAtivo { get; set; }
    public decimal Quantidade { get; set; }
    public decimal PrecoUni { get; set; }
    public TipoOperacao Tipo { get; set; }
    public decimal Corretagem { get; set; }
    public DateTime DtHora { get; set; }
}