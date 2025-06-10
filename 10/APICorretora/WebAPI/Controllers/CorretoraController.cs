using Microsoft.AspNetCore.Mvc;
using WebAPI.Application.Services;
using WebAPI.Domain;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CorretoraController : ControllerBase
    {
        private readonly ILogger<CorretoraController> _logger;
        private CorretoraService _corretoraService;
        public CorretoraController(CorretoraService corretoraService)
        {
            _corretoraService = corretoraService;
        }

        [HttpGet("cotacaoAtivos{codAtivo}")]
        public async Task<ActionResult<Cotacao>> GetCotacaoAtivos(string codAtivo)
        {
            try
            {
                var cotacao = await _corretoraService.GetUltimaCotacaoAsync(codAtivo);

                if (cotacao == null)
                    return NotFound($"Cotação para o ativo '{codAtivo}' não encontrada.");

                return Ok(cotacao);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cotação do ativo.");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        [HttpGet("corretagem-total")]
        public async Task<ActionResult<decimal>> GetTotalCorretagem()
        {
            try
            {
                var total = await _corretoraService.GetTotalCorretagemAsync();
                return Ok(total);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "Erro ao calcular total de corretagem.");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

    }
}