using WorkerService.Application.Consumers;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            using (var scope = _scopeFactory.CreateScope())
            {
                var consumerService = scope.ServiceProvider.GetRequiredService<CotacaoConsumerService>();
                await consumerService.ProcessarMensagensAsync(stoppingToken);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}