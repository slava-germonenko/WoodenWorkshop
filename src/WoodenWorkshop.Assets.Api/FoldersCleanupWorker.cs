using WoodenWorkshop.Assets.Core;

namespace WoodenWorkshop.Assets.Api;

public class FoldersCleanupWorker : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly TimeSpan _cleanupDelay = TimeSpan.FromMinutes(30);

    public FoldersCleanupWorker(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await RunCleanupSafe();
            await Task.Delay(_cleanupDelay, stoppingToken);
        }
    }

    private async Task RunCleanupSafe()
    {
        try
        {
            var cleanupService = _serviceScopeFactory.CreateScope()
                .ServiceProvider
                .GetRequiredService<FoldersCleanupService>();

            await cleanupService.CleanupFoldersAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // Add logging here
        }
    }
}