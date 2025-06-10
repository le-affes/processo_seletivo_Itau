using ControleUsuario.Infra;
using Microsoft.EntityFrameworkCore;
using WebAPI.Domain;

namespace WebAPI.Application.Services;

public class CorretoraService
{
    private readonly ILogger<CorretoraService> _logger;
    private readonly DatabaseContext _context;

    public CorretoraService(DatabaseContext context, ILogger<CorretoraService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Cotacao> GetUltimaCotacaoAsync(string codAtivo)
    {
        try
        {
            var ativo = await _context.Ativos.FirstOrDefaultAsync(a => a.Codigo == codAtivo);
            if (ativo == null)
                throw new ArgumentException($"Ativo com código {codAtivo} não encontrado.");

            var cotacao = await _context.Cotacoes
                .Where(c => c.IdAtivo == ativo.Id)
                .OrderByDescending(c => c.DtHora)
                .FirstOrDefaultAsync();

            return cotacao; // pode ser null se não houver cotação
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar última cotação");
            throw; // relança para ser tratado no controller
        }
    }

    public async Task<decimal> GetTotalCorretagemAsync()
    {
        try
        {
            return await _context.Operacoes.SumAsync(op => op.Corretagem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao calcular total de corretagem");
            throw new Exception("Erro ao calcular total de corretagem.", ex);
        }
    }
}