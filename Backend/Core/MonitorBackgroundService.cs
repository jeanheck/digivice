using Microsoft.Extensions.Hosting;

namespace Backend.Core
{
    /// <summary>
    /// Hosted background service that runs the AppMonitor loop
    /// alongside the WebApplication host, using the idiomatic ASP.NET Core pattern.
    /// </summary>
    public class MonitorBackgroundService(AppMonitor monitor) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => Task.Run(monitor.Run, stoppingToken);
    }
}
