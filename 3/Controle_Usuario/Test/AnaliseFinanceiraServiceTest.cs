using ControleUsuario.Application.Interfaces;
using ControleUsuario.Application.Services;
using ControleUsuario.Infra;
using ControleUsuario.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Test;

[TestClass]
public sealed class AnaliseFinanceiraServiceTest
{
    private ServiceProvider _serviceProvider;
    private IAnaliseFinanceiraService _analiseFinanceiraService;

    [TestInitialize]
    public void Setup()
    {
        var services = new ServiceCollection();
        services.AddDbContext<OperacaoDbContext>(options =>
        {
            options.UseInMemoryDatabase("TesteDb"); 
        });
        services.AddScoped<IAnaliseFinanceiraService, AnaliseFinanceiraService>();

        _serviceProvider = services.BuildServiceProvider();
        _analiseFinanceiraService = _serviceProvider.GetRequiredService<IAnaliseFinanceiraService>();
    }

    [TestMethod]
    public void CalcularPrecoMedio_ComUmaCompra()
    {
        var compras = new List<Operacao>
    {
        new Operacao { Quantidade = 10, PrecoUni = 20m, Tipo = TipoOperacao.C }
    };

        var resultado = _analiseFinanceiraService.CalcularPrecoMedio(compras);

        Assert.AreEqual(20m, resultado);
    }

    [TestMethod]
    public void CalcularPrecoMedio_ComMultiplasCompras()
    {
        var compras = new List<Operacao>
    {
        new Operacao { Quantidade = 5, PrecoUni = 10m, Tipo = TipoOperacao.C },
        new Operacao { Quantidade = 15, PrecoUni = 20m, Tipo = TipoOperacao.C }
    };

        var resultado = _analiseFinanceiraService.CalcularPrecoMedio(compras);

        Assert.AreEqual(17.5m, resultado);
    }

    [TestMethod]
    public void CalcularPrecoMedio_IgnoraOperacoesDeVenda()
    {
        var compras = new List<Operacao>
    {
        new Operacao { Quantidade = 10, PrecoUni = 10m, Tipo = TipoOperacao.C },
        new Operacao { Quantidade = 10, PrecoUni = 50m, Tipo = TipoOperacao.V }
    };

        var resultado = _analiseFinanceiraService.CalcularPrecoMedio(compras);

        Assert.AreEqual(10m, resultado);
    }

    [TestMethod]
    public void CalcularPrecoMedio_ComOperacaoNula()
    {
        var compras = new List<Operacao>
    {
        new Operacao { Quantidade = 10, PrecoUni = 30m, Tipo = TipoOperacao.C },
        null
    };

        var resultado = _analiseFinanceiraService.CalcularPrecoMedio(compras);

        Assert.AreEqual(30m, resultado);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CalcularPrecoMedio_ListaNula_DisparaExcecao()
    {
        List<Operacao> compras = null;

        _analiseFinanceiraService.CalcularPrecoMedio(compras);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CalcularPrecoMedio_ListaVazia_DisparaExcecao()
    {
        var compras = new List<Operacao>();

        _analiseFinanceiraService.CalcularPrecoMedio(compras);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CalcularPrecoMedio_QuantidadeInvalida_DisparaExcecao()
    {
        var compras = new List<Operacao>
    {
        new Operacao { Quantidade = 0, PrecoUni = 10m, Tipo = TipoOperacao.C }
    };

        _analiseFinanceiraService.CalcularPrecoMedio(compras);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CalcularPrecoMedio_SomenteVendas_DisparaExcecao()
    {
        var compras = new List<Operacao>
    {
        new Operacao { Quantidade = 10, PrecoUni = 10m, Tipo = TipoOperacao.V }
    };

        _analiseFinanceiraService.CalcularPrecoMedio(compras);
    }

}
