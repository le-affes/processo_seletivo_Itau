using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.Dtos;
using WebAPI.Application.Services;
using WebAPI.Domain;

namespace WebAPI.Controllers;

public class UsuarioController : ControllerBase
{
    private readonly ILogger<CorretoraController> _logger;
    private UsuarioService _usuarioService;
    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("{idUsr}/preco-medio-ativo")]
    public async Task<ActionResult<List<PrecoMedioDTO>>> GetCotacaoAtivos(int idUsr)
    {
        try
        {
            var resultado = await _usuarioService.GetPrecoMedioPorAtivoAsync(idUsr);

            if (resultado is null || !resultado.Any())
                return NotFound($"Nenhum preço médio encontrado para o usuário com ID {idUsr}.");

            var dto = resultado.Select(r => new PrecoMedioDTO
            {
                IdAtivo = r.IdAtivo,
                PrecoMedio = r.PrecoMedio
            }).ToList();

            return Ok(dto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar preço médio por ativo.");
            return StatusCode(500, "Erro interno no servidor.");
        }
    }


    [HttpGet("{idUsr}/posicao-usuario")]
    public async Task<ActionResult<List<PosicaoDto>>> GetPosicaosAsync(int idUsr, string codAtivo)
    {
        try
        {
            var posicoes = await _usuarioService.GetPosicaoPorUsuarioAsync(idUsr, codAtivo);

            if (posicoes == null || !posicoes.Any())
                return NotFound($"Nenhuma posição encontrada para o usuário {idUsr} com o ativo '{codAtivo}'.");

            return Ok(posicoes);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
           _logger.LogError(ex, "Erro ao buscar posições do usuário.");
            return StatusCode(500, "Erro interno no servidor.");
        }
    }


    [HttpGet("clientes-top-pl")]
    public async Task<ActionResult<List<TopClienteDTO>>> GetTopClientes(int topClientes = 10)
    {
        try
        {
            if (topClientes <= 0)
                return BadRequest("O parâmetro 'topClientes' deve ser maior que zero.");

            var topClientesList = await _usuarioService.GetTopUsuariosPLAsync(topClientes);

            if (topClientesList == null || !topClientesList.Any())
                return NotFound("Nenhum cliente encontrado.");

            return Ok(topClientesList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar top clientes.");
            return StatusCode(500, "Erro interno no servidor.");
        }
    }


    [HttpGet("clientes-top-corretagem")]
    public async Task<ActionResult<List<TopCorretagemDto>>> GetTop10ClientesPorCorretagem(int topClientes = 10)
    {
        try
        {
            if (topClientes <= 0)
                return BadRequest("O parâmetro 'topClientes' deve ser maior que zero.");

            var clientes = await _usuarioService.GetTopUsuariosCorretagemAsync(topClientes);

            if (clientes == null || !clientes.Any())
                return NotFound("Nenhum cliente encontrado.");

            return Ok(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar top clientes por corretagem.");
            return StatusCode(500, "Erro interno no servidor.");
        }
    }
}