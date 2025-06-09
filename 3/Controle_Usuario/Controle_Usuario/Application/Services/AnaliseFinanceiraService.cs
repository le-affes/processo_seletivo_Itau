using ControleUsuario.Application.Interfaces;
using ControleUsuario.Infra;
using ControleUsuario.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleUsuario.Application.Services;

public class AnaliseFinanceiraService : IAnaliseFinanceiraService
{
    private readonly OperacaoDbContext _context;
    private readonly ILogger<AnaliseFinanceiraService> _logger;

    public AnaliseFinanceiraService(OperacaoDbContext context, ILogger<AnaliseFinanceiraService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public decimal CalcularPrecoMedio(List<Operacao> compras)
    {
        try
        {
            if (compras == null || compras.Count == 0)
                throw new ArgumentException("A lista de compras não pode ser nula ou vazia.");

            var comprasValidas = compras
                .Where(c => c != null && c.Tipo == TipoOperacao.C && c.Quantidade > 0 && c.PrecoUni > 0)
                .ToList();

            if (!comprasValidas.Any())
                throw new InvalidOperationException("Não existem compras válidas para calcular o preço médio.");

            decimal quantidadeTotal = comprasValidas.Sum(c => c.Quantidade);
            decimal valorTotal = comprasValidas.Sum(c => c.Quantidade * c.PrecoUni);

            if (quantidadeTotal == 0)
                throw new InvalidOperationException("Quantidade total é zero, impossível calcular preço médio.");

            return valorTotal / quantidadeTotal;
        }
        catch (DivideByZeroException ex)
        {
            throw new InvalidOperationException("Erro ao calcular preço médio: divisão por zero.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao calcular preço médio.");
            throw new Exception("Erro inesperado ao calcular preço médio.", ex);
        }
    }

    async Task<List<Ativo>> IAnaliseFinanceiraService.GetAtivosAsync()
    {
        try
        {
            return await _context.Ativos.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter a lista de ativos.");
            throw;
        }
    }
}