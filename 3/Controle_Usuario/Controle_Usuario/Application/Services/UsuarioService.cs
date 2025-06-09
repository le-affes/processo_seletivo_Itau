using ControleUsuario.Infra;
using ControleUsuario.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleUsuario.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly OperacaoDbContext _context;

    public UsuarioService(OperacaoDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, decimal>> GetInvestidoPorAtivoAsync(int idUsuario)
    {
        var totalInvestidoPorAtivo = await _context.Operacoes
            .Where(op => op.IdUsuario == idUsuario && op.Tipo == TipoOperacao.C)
            .GroupBy(op => op.IdAtivo)
            .Select(group => new
            {
                IdAtivo = group.Key,
                TotalInvestido = group.Sum(op => op.Quantidade * op.PrecoUni)
            })
            .Join(_context.Ativos,
                  g => g.IdAtivo,
                  ativo => ativo.Id,
                  (g, ativo) => new { ativo.Nome, g.TotalInvestido })
            .ToDictionaryAsync(x => x.Nome, x => x.TotalInvestido);

        return totalInvestidoPorAtivo;
    }

    public async Task<Dictionary<string, decimal>> GetPosicoesPorUsuarioAsync(int idUsuario)
    {
        var resultado = await (from p in _context.Posicoes
                               join a in _context.Ativos on p.IdAtivo equals a.Id
                               where p.IdUsuario == idUsuario
                               select new { a.Codigo, p.Quantidade })
                             .ToListAsync();

        return resultado.ToDictionary(x => x.Codigo, x => x.Quantidade);
    }

    public async Task<decimal> GetPLTotalAsync(int idUsuario)
    {
        return await _context.Posicoes
            .Where(p => p.IdUsuario == idUsuario)
            .SumAsync(p => (decimal?)p.PL) ?? 0m;
    }

    public async Task<decimal> GetTotalCorretagemAsync(int idUsuario)
    {
        return await _context.Operacoes
            .Where(o => o.IdUsuario == idUsuario)
            .SumAsync(o => (decimal?)o.Corretagem) ?? 0m;
    }

    public async Task<List<Usuario>> GetUsuariosAsync()
    {
        return await _context.Usuarios.AsNoTracking().ToListAsync();
    }
}