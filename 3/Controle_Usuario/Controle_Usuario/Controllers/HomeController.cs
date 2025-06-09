using ControleUsuario.Application.Dtos;
using ControleUsuario.Application.Interfaces;
using ControleUsuario.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ControleUsuario.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUsuarioService _usuarioService;
    private readonly IAnaliseFinanceiraService _analiseService;
    public HomeController(ILogger<HomeController> logger, IUsuarioService usuarioService, IAnaliseFinanceiraService analiseService)
    {
        _logger = logger;
        _usuarioService = usuarioService;
        _analiseService = analiseService;
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public async Task<IActionResult> IndexAsync(int? usuarioId)
    {
        if (!usuarioId.HasValue)
            return View(null);

        try
        {
            var totalInvestido = await _usuarioService.GetInvestidoPorAtivoAsync(usuarioId.Value);
            var posicaoPapel = await _usuarioService.GetPosicoesPorUsuarioAsync(usuarioId.Value);
            var posicaoGlobal = await _usuarioService.GetPLTotalAsync(usuarioId.Value);
            var totalCorretagem = await _usuarioService.GetTotalCorretagemAsync(usuarioId.Value);

            var resumoInvestidor = new ResumoInvestidorDto
            {
                TotalInvestidoPorAtivo = totalInvestido,
                PosicaoPorPapel = posicaoPapel,
                PosicaoGlobal = posicaoGlobal,
                TotalCorretagem = totalCorretagem
            };
            return View(resumoInvestidor);
        }
        catch (Exception ex)
        {
            var errorModel = new ErrorViewModel
            {
                Message = "Erro ao carregar dados do investidor.",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            _logger.LogError(ex, "Erro no IndexAsync");
            return View("Error", errorModel);
        }
    }

    public async Task<IActionResult> ListaUsuariosAsync()
    {
        try
        {
            var usuarios = (await _usuarioService.GetUsuariosAsync()).ToList();
            return View(usuarios);
        }
        catch (Exception ex)
        {
            var errorModel = new ErrorViewModel
            {
                Message = "Erro ao carregar a lista de usuários.",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            _logger.LogError(ex, "Erro em ListaUsuariosAsync");
            return View("Error", errorModel);
        }
    }


    public async Task<IActionResult> ListaAtivosAsync()
    {
        try
        {
            var ativos = (await _analiseService.GetAtivosAsync()).ToList();
            return View(ativos);
        }
        catch (Exception ex)
        {
            var errorModel = new ErrorViewModel
            {
                Message = "Erro ao carregar a lista de ativos.",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            
            _logger.LogError(ex, "Erro em ListaAtivosAsync");
            return View("Error", errorModel);
        }
    }
}