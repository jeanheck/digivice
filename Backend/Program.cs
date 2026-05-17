using Backend.Diagnostics;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Digimon;
using Backend.Memory.Readers.Quests;
using Backend.Memory.Readers.Party;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Backend.Events.Hubs;
using Backend.Events.Services;
using Backend.Events.Generation;
using Backend.Application.Services;
using Backend.Application.Providers;
using Backend.Application.Loaders;
using Backend.Memory.Repositories;
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
    builder.Services.AddSingleton<IMemoryReader, MemoryReader>();
    builder.Services.AddSingleton<IAddressesRepository, AddressesRepository>();
    builder.Services.AddSingleton<IDigimonReader, DigimonReader>();
    builder.Services.AddSingleton<IDigimonSlotReader, DigimonSlotReader>();
    builder.Services.AddSingleton<IPartyReader, PartyReader>();
    builder.Services.AddSingleton<IRequisiteReader, RequisiteReader>();
    builder.Services.AddSingleton<IStepReader, StepReader>();
    builder.Services.AddSingleton<IPlayerReader, PlayerReader>();
    builder.Services.AddSingleton<IQuestReader, QuestReader>();
    builder.Services.AddSingleton<PlayerLoader>();
    builder.Services.AddSingleton<QuestLoader>();
    builder.Services.AddSingleton<JournalLoader>();
    builder.Services.AddSingleton<PartyLoader>();
    builder.Services.AddSingleton<PlayerProvider>();
    builder.Services.AddSingleton<DigievolutionStateService>();
    builder.Services.AddSingleton<DigimonStateService>();
    builder.Services.AddSingleton<PartyProvider>();
    builder.Services.AddSingleton<JournalProvider>();
    builder.Services.AddSingleton<StateProvider>();
    builder.Services.AddSingleton<DebugConsoleRenderer>();

    // Register Event Dispatcher
    builder.Services.AddSingleton<StateEventGenerator>();
    builder.Services.AddSingleton<IEventDispatcherService, EventDispatcherService>();

    // Start the Background Monitor (Memory Reader)
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
