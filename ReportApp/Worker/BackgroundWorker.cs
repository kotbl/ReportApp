using BLL.Services;
using BLL.Services.Interfaces;

namespace API.Worker;

public class BackgroundWorker : BackgroundService
{
    private readonly BackgroundWorkerQueue _backgroundWorkerQueue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BackgroundWorker> _logger;

    public BackgroundWorker(
        BackgroundWorkerQueue backgroundWorkerQueue, 
        IServiceProvider serviceProvider, 
        ILogger<BackgroundWorker> logger)
    {
        _backgroundWorkerQueue = backgroundWorkerQueue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var queueItem = await _backgroundWorkerQueue.DequeueAsync(stoppingToken);
            if (queueItem is null)
                return;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var queryService = scope.ServiceProvider.GetService<IReportProcessingService>();
                await queryService!.Process(queueItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing {QueueItem}.", nameof(queueItem));
            }
        }
    }
}
