using Backend.Events.Hubs;
using Backend.Infrastructure;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.Console()
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

    // Register backend services modularly
    builder.Services.AddBackendServices(basePath);

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
