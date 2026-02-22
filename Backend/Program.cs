using Backend.Core;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Backend.Interfaces;
using Backend.UI;
using Backend.Events.Hubs;
using Backend.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Initializing Digivice Backend...");

    var builder = WebApplication.CreateBuilder(args);

    // Use Serilog for all framework logging
    builder.Host.UseSerilog();

    // Register game dependencies
    builder.Services.AddSingleton<IProcessService, WindowsProcessProvider>();
    builder.Services.AddSingleton<IMemoryProvider, WindowsMemoryProvider>();
    builder.Services.AddSingleton<ConsoleRenderer>();
    builder.Services.AddSingleton<Backend.Core.Monitor>();

    // Register the game monitor as a hosted background service
    builder.Services.AddHostedService<MonitorBackgroundService>();

    // Register SignalR
    builder.Services.AddSignalR();

    var app = builder.Build();

    // Map the WebSocket endpoint
    app.MapHub<GameHub>("/gamehub");

    Log.Information("SignalR Hub available at ws://localhost:5000/gamehub");

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly.");
}
finally
{
    await Log.CloseAndFlushAsync();
}
