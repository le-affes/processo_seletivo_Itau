namespace WorkerService.Domain;

public class Posicao
{
    public int Id { get; set; }
    public int IdUsuario { get; set; }
    public int IdAtivo { get; set; }
    public decimal Quantidade { get; set; }
    public decimal PrecoMedio { get; set; }
    public decimal PL { get; set; }
}