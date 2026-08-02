using Backend.Events.Hubs;
using Backend.Infrastructure;
using Serilog;

HashSet<string> allowedOrigins = new(StringComparer.OrdinalIgnoreCase)
{
    "http://tauri.localhost",
    "https://tauri.localhost",
    "tauri://localhost",
};

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

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

    builder.Host.UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    // Register backend services modularly
    builder.Services.AddBackendServices(basePath);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowLocalhost", policy =>
        {
            policy.SetIsOriginAllowed(origin =>
                  {
                      if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                          return false;

                      if (uri.IsLoopback)
                          return true;

                      return allowedOrigins.Contains(origin);
                  })
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
    Environment.ExitCode = 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}
