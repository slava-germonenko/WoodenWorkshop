using Microsoft.Extensions.Options;

using WoodenWorkshop.Sessions.Core;

namespace WoodenWorkshop.Sessions.Api;

public class SessionsCleanupWorker : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private IOptionsSnapshot<Core.Options.SessionsOptions> SessionsOptions => _serviceScopeFactory.CreateScope()
        .ServiceProvider
        .GetRequiredService<IOptionsSnapshot<Core.Options.SessionsOptions>>();

    private TimeSpan CleanupDelay => TimeSpan.FromMinutes(SessionsOptions.Value.CleanupIntervalMinutes);

    public SessionsCleanupWorker(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await RunCleanupCycle(); 
            await Task.Delay(CleanupDelay, stoppingToken);
        }
    }

    private async Task RunCleanupCycle()
    {
        try
        {
            var cleanupService = _serviceScopeFactory.CreateScope()
                .ServiceProvider
                .GetRequiredService<SessionsCleanUpService>();

            await cleanupService.CleanupExpiredSessionsAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // Add logging here
        }
    }
}