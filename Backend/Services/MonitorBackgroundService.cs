using Microsoft.Extensions.Hosting;
using Backend.Core;

namespace Backend.Services
{
    /// <summary>
    /// Hosted background service that runs the AppMonitor loop
    /// alongside the WebApplication host, using the idiomatic ASP.NET Core pattern.
    /// </summary>
    public class MonitorBackgroundService(Backend.Core.Monitor monitor) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => Task.Run(monitor.Run, stoppingToken);
    }
}
