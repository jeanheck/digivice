using Backend.Diagnostics;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Backend.Events.Hubs;
using Backend.Events.Interfaces;
using Backend.Events.Services;
using Backend.Interfaces;
using Backend.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.Console()
    .WriteTo.File("logs/backend-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: null,
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Initializing Digivice Backend...");

    // Set base directory appropriately for single file executable / sidecar execution
    var basePath = AppContext.BaseDirectory;
    Directory.SetCurrentDirectory(basePath);

    var builder = WebApplication.CreateBuilder(new WebApplicationOptions
    {
        Args = args,
        ContentRootPath = basePath
    });

    // Use Serilog for all framework logging
    builder.Host.UseSerilog();

    // Register game dependencies
    builder.Services.AddSingleton<IProcessService, WindowsProcessProvider>();
    builder.Services.AddSingleton<IMemoryProvider, WindowsMemoryProvider>();
    builder.Services.AddSingleton<IMemoryReaderService, MemoryReaderService>();
    builder.Services.AddSingleton<IGameDatabase, GameDatabase>();
    builder.Services.AddSingleton<IGameReader, GameReader>();
    builder.Services.AddSingleton<PlayerStateService>();
    builder.Services.AddSingleton<DigievolutionStateService>();
    builder.Services.AddSingleton<DigimonStateService>();
    builder.Services.AddSingleton<PartyStateService>();
    builder.Services.AddSingleton<ItemsStateService>();
    builder.Services.AddSingleton<JournalStateService>();
    builder.Services.AddSingleton<GameStateService>();
    builder.Services.AddSingleton<DebugConsoleRenderer>();

    // Register Event Dispatcher
    builder.Services.AddSingleton<IEventDispatcherService, EventDispatcherService>();

    // Start the Background Monitor (Memory Reader)
    Log.Information("Activating Game Memory Poller in Background...");
    builder.Services.AddHostedService<GameLoopBackgroundService>();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowLocalhost", policy =>
        {
            policy.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback || origin.Contains("tauri"))
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

    app.Lifetime.ApplicationStarted.Register(() =>
    {
        Log.Information("SignalR Hub available at {Urls}/gamehub", string.Join(", ", app.Urls));
    });

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
