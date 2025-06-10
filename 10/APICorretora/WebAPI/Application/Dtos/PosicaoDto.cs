namespace WebAPI.Application.Dtos;

public class PosicaoDto
{
    public string CodigoAtivo { get; set; }
    public decimal Quantidade { get; set; }
    public decimal PrecoMedio { get; set; }
    public decimal PL { get; set; }
}