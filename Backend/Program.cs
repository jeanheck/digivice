using Backend.Debug;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Backend.Events.Hubs;
using Backend.Events.Interfaces;
using Backend.Events.Services;
using Backend.Interfaces;
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
    builder.Services.AddSingleton<IMemoryReaderService, MemoryReaderService>();
    builder.Services.AddSingleton<GameDatabase>();
    builder.Services.AddSingleton<GameReader>();
    builder.Services.AddSingleton<PlayerStateService>();
    builder.Services.AddSingleton<PartyStateService>();
    builder.Services.AddSingleton<ItemStateService>();
    builder.Services.AddSingleton<GameStateService>();
    builder.Services.AddSingleton<DebugConsoleRenderer>();
    builder.Services.AddSingleton<DebugMonitor>();

    // Register Event Dispatcher
    builder.Services.AddSingleton<IEventDispatcherService, EventDispatcherService>();

    // Read the Debugging feature flag from appsettings.json
    bool isDebugging = builder.Configuration.GetValue<bool>("Features:Debugging", false);

    if (isDebugging)
    {
        Log.Information("Debugging flag detected! Activating Background Monitor...");
        builder.Services.AddHostedService<DebugMonitorBackgroundService>();
    }

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowLocalhost", policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:5173/")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Required for SignalR
        });
    });

    // Register SignalR
    builder.Services.AddSignalR();

    var app = builder.Build();

    app.UseCors("AllowLocalhost");

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
