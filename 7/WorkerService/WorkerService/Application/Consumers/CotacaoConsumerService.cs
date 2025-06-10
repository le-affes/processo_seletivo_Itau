using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.CircuitBreaker;
using System.Text.Json;
using WorkerService.Domain;
using WorkerService.Infra;

namespace WorkerService.Application.Consumers;

public class CotacaoConsumerService
{
    private readonly ILogger<CotacaoConsumerService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly IAsyncPolicy _resiliencePolicy;
    private readonly CotacaoDbContext _context;
    private readonly AssetApiConsumer _assetApiConsumer;
    private List<Ativo> ativos;
    public CotacaoConsumerService(
        ILogger<CotacaoConsumerService> logger,
        IServiceScopeFactory scopeFactory,
        CotacaoDbContext context,
        AssetApiConsumer assetApiConsumer)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _context = context;

        var config = new ConsumerConfig
        {
            BootstrapServers = string.Empty, // <-- Adicione seu bootstrap aqui
            GroupId = "cotacoes-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe("cotacoes-consumer-group"); // <-- Nome do tópico (não grupo)

        // Circuit Breaker Policy
        var circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .AdvancedCircuitBreakerAsync(
                failureThreshold: 0.5,
                samplingDuration: TimeSpan.FromSeconds(10),
                minimumThroughput: 10,
                durationOfBreak: TimeSpan.FromSeconds(15),
                onBreak: (ex, breakDelay) =>
                {
                    _logger.LogWarning($"Circuit breaker aberto por {breakDelay.TotalSeconds} segundos devido ao erro: {ex.Message}");
                },
                onReset: () =>
                {
                    _logger.LogInformation("Circuit breaker resetado.");
                },
                onHalfOpen: () =>
                {
                    _logger.LogInformation("Circuit breaker em estado Half-Open.");
                });

        var fallbackPolicy = Policy
            .Handle<Exception>()
            .FallbackAsync(
                fallbackAction: async (context) =>
                {
                    await ExecutarFallbackAsync();
                },
                onFallbackAsync: async (outcome) =>
                {
                    _logger.LogWarning("Fallback acionado. Erro: {Erro}", outcome.Message);
                    await Task.CompletedTask;
                });

        // Política de resiliência final
        _resiliencePolicy = Policy.WrapAsync(circuitBreakerPolicy, fallbackPolicy);
        _assetApiConsumer = assetApiConsumer;
    }

    private async Task ExecutarFallbackAsync()
    {
        _logger.LogWarning("Executando fallback: o circuito está aberto ou uma falha foi detectada.");

        try
        {
            var assets = await _assetApiConsumer.GetAssetsAsync();

            using var scope = _scopeFactory.CreateScope();
            var scopedContext = scope.ServiceProvider.GetRequiredService<CotacaoDbContext>();

            if (assets is null || !assets.Any())
            {
                _logger.LogWarning("Nenhum ativo encontrado na API externa. Abortando fallback.");
                return;
            }

            assets = assets.Where(a => a.Ticker != null && a.Price != null && a.TradeTime != null).ToList();

            foreach (var asset in assets)
            {
                // Mapear AssetDto para Cotacao (supondo que Cotacao tenha campos equivalentes)
                var cotacao = new Cotacao
                {
                    IdAtivo = ativos.FirstOrDefault(x => x.Codigo == asset.Ticker).Id,
                    PrcoUni = asset.Price.Value,
                    DtHora = asset.TradeTime.Value
                };

                bool jaExiste = await scopedContext.Cotacoes.AnyAsync(c =>
                    c.IdAtivo == cotacao.IdAtivo &&
                    c.DtHora == cotacao.DtHora);

                if (!jaExiste)
                {
                    await scopedContext.Cotacoes.AddAsync(cotacao);
                }
            }

            await scopedContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar fallback e salvar cotações da API externa.");
        }
    }

    public async Task ProcessarMensagensAsync(CancellationToken stoppingToken)
    {
        ativos = _context.Ativos.ToList();
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _resiliencePolicy.ExecuteAsync(async () =>
                {
                    var result = _consumer.Consume(TimeSpan.FromMilliseconds(100));

                    if (result is null)
                        throw new Exception("Nenhuma mensagem recebida do Kafka.");

                    var cotacao = JsonSerializer.Deserialize<Cotacao>(result.Message.Value);

                    if (cotacao is null)
                        throw new Exception("Falha ao desserializar cotação.");

                    using var scope = _scopeFactory.CreateScope();
                    var scopedContext = scope.ServiceProvider.GetRequiredService<CotacaoDbContext>();

                    bool jaExiste = await scopedContext.Cotacoes.AnyAsync(c =>
                        c.IdAtivo == cotacao.IdAtivo &&
                        c.DtHora == cotacao.DtHora);

                    if (!jaExiste)
                    {
                        await scopedContext.Cotacoes.AddAsync(cotacao);
                        await scopedContext.SaveChangesAsync();
                        _logger.LogInformation($"Cotação salva: [Ativo] = {cotacao.IdAtivo}, [Preço] = {cotacao.PrcoUni}, [Hora] = {cotacao.DtHora}");
                    }
                    else
                    {
                        _logger.LogInformation($"Cotação já existente ignorada: [Ativo] = {cotacao.IdAtivo}, [Hora] = {cotacao.DtHora}");
                    }

                    _consumer.Commit(result);
                });
            }
            catch (BrokenCircuitException ex)
            {
                _logger.LogWarning("Circuito aberto - ignorando tentativa de consumir cotação: {Message}", ex.Message);
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex, "Erro ao consumir mensagem do Kafka.");
                throw;
            }
            catch (KafkaException ex) when (ex.Error.Code == ErrorCode.UnknownTopicOrPart)
            {
                _logger.LogError(ex, "Tópico Kafka desconhecido.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado no processamento da cotação.");
                throw;
            }
        }
    }
}