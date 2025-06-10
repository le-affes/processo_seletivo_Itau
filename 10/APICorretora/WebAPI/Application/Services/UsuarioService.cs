using ControleUsuario.Infra;
using Microsoft.EntityFrameworkCore;
using WebAPI.Application.Dtos;
using WebAPI.Domain;

namespace WebAPI.Application.Services;

public class UsuarioService
{
    private readonly ILogger<UsuarioService> _logger;
    private readonly DatabaseContext _context;
    public UsuarioService(DatabaseContext context, ILogger<UsuarioService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<(int IdAtivo, decimal PrecoMedio)>> GetPrecoMedioPorAtivoAsync(int idUsuario)
    {
        try
        {
            var precosMedios = await _context.Operacoes
                .Where(op => op.IdUsuario == idUsuario && op.Tipo == TipoOperacao.C)
                .GroupBy(op => op.IdAtivo)
                .Select(g => new
                {
                    IdAtivo = g.Key,
                    PrecoMedio = g.Sum(op => op.PrecoUni * op.Quantidade) / g.Sum(op => op.Quantidade)
                })
                .ToListAsync();

            return precosMedios
                .Select(p => (p.IdAtivo, p.PrecoMedio))
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao calcular preço médio por ativo");
            throw new Exception("Erro ao calcular preço médio por ativo.", ex);
        }
    }

    public async Task<List<PosicaoDto>> GetPosicaoPorUsuarioAsync(int idUsuario, string? codAtivo = null)
    {
        try
        {
            var query = from p in _context.Posicoes
                        join a in _context.Ativos on p.IdAtivo equals a.Id
                        where p.IdUsuario == idUsuario
                        select new { a.Codigo, p.Quantidade, p.PrecoMedio, p.PL };

            if (!string.IsNullOrEmpty(codAtivo))
            {
                query = query.Where(x => x.Codigo == codAtivo);
            }

            var resultado = await query
                .Select(x => new PosicaoDto
                {
                    CodigoAtivo = x.Codigo,
                    Quantidade = x.Quantidade,
                    PrecoMedio = x.PrecoMedio,
                    PL = x.PL
                })
                .ToListAsync();

            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar posições por usuário");
            throw new Exception("Erro ao buscar posições por usuário.", ex);
        }
    }

    public async Task<List<TopClienteDTO>> GetTopUsuariosPLAsync(int usrTop)
    {
        try
        {
            var topUsr = await _context.Posicoes
                .GroupBy(p => p.IdUsuario)
                .Select(g => new
                {
                    IdUsuario = g.Key,
                    ValorTotalPL = g.Sum(p => p.PL)
                })
                .OrderByDescending(x => x.ValorTotalPL)
                .Take(usrTop)
                .Join(_context.Usuarios,
                      g => g.IdUsuario,
                      u => u.Id,
                      (g, u) => new TopClienteDTO
                      {
                          IdUsuario = u.Id,
                          NomeUsuario = u.Nome,
                          ValorTotalPL = g.ValorTotalPL
                      })
                .ToListAsync();

            return topUsr;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar top usuários por PL");
            throw new Exception("Erro ao buscar top usuários por PL.", ex);
        }
    }

    public async Task<List<TopCorretagemDto>> GetTopUsuariosCorretagemAsync(int usrTop)
    {
        try
        {
            var topCorretagens = await _context.Operacoes
                .GroupBy(op => op.IdUsuario)
                .Select(g => new
                {
                    IdUsuario = g.Key,
                    TotalCorretagem = g.Sum(op => op.Corretagem)
                })
                .OrderByDescending(x => x.TotalCorretagem)
                .Take(usrTop)
                .Join(_context.Usuarios,
                      g => g.IdUsuario,
                      u => u.Id,
                      (g, u) => new TopCorretagemDto
                      {
                          IdUsuario = u.Id,
                          NomeUsuario = u.Nome,
                          TotalCorretagem = g.TotalCorretagem
                      })
                .ToListAsync();

            return topCorretagens;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar top usuários por corretagem");
            throw new Exception("Erro ao buscar top usuários por corretagem.", ex);
        }
    }
}