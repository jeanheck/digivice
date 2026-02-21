using Backend.Interfaces;
using Backend.Services;
using Backend.Core;
using Backend.UI;
using Serilog;

namespace Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Initializing Digivice...");

            var processService = new WindowsProcessService();
            var memoryProvider = new WindowsMemoryProvider();
            var renderer = new ConsoleRenderer();

            var monitor = new AppMonitor(processService, memoryProvider, renderer);
            monitor.Run();

            Log.Information("Application ended.");
        }
    }
}
