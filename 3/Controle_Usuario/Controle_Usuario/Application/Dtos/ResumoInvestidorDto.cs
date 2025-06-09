namespace ControleUsuario.Application.Dtos;
public class ResumoInvestidorDto
{
    public Dictionary<string, decimal> TotalInvestidoPorAtivo { get; set; }
    public Dictionary<string, decimal > PosicaoPorPapel { get; set; }
    public decimal PosicaoGlobal { get; set; }
    public decimal TotalCorretagem { get; set; }
}