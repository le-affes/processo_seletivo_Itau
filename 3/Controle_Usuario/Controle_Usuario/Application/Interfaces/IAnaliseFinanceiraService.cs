using ControleUsuario.Models;

namespace ControleUsuario.Application.Interfaces;

public interface IAnaliseFinanceiraService
{
    decimal CalcularPrecoMedio(List<Operacao> compras);
    Task<List<Ativo>> GetAtivosAsync();
}