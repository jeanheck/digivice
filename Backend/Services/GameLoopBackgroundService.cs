using Backend.Diagnostics;
using Backend.Events.Interfaces;
using Backend.Interfaces;

namespace Backend.Services
{
    public class GameLoopBackgroundService : BackgroundService
    {
        private readonly IMemoryReaderService _readerService;
        private readonly GameStateService _gameStateService;
        private readonly IEventDispatcherService _dispatcherService;
        private readonly DebugConsoleRenderer _debugConsoleRenderer;
        private readonly IConfiguration _configuration;

        public GameLoopBackgroundService(
            IMemoryReaderService readerService,
            GameStateService gameStateService,
            IEventDispatcherService dispatcherService,
            DebugConsoleRenderer debugConsoleRenderer,
            IConfiguration configuration)
        {
            _readerService = readerService;
            _gameStateService = gameStateService;
            _dispatcherService = dispatcherService;
            _debugConsoleRenderer = debugConsoleRenderer;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Serilog.Log.Information("Starting GameLoopBackgroundService...");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!_readerService.IsConnected)
                {
                    if (!_readerService.TryConnect())
                    {
                        // Se falhar em conectar, despacha false e espera 1s antes de tentar novamente
                        _dispatcherService.DispatchConnectionStatus(false);
                        await Task.Delay(1000, stoppingToken);
                        continue;
                    }
                    else
                    {
                        // Conectou com sucesso
                        Serilog.Log.Information("Connected to DuckStation.");
                        _dispatcherService.DispatchConnectionStatus(true);
                    }
                }

                try
                {
                    var state = _gameStateService.GetState();

                    if (state?.Player != null)
                    {
                        // Dispatch any state differences
                        _dispatcherService.ProcessGameState(state);

                        // Render debugging console if flag is enabled
                        var isDebuggingEnabled = _configuration.GetValue<bool>("Features:Debugging");
                        if (isDebuggingEnabled && !Console.IsOutputRedirected)
                        {
                            _debugConsoleRenderer.Render(state);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ignora o erro se a aplicação estiver sendo desligada
                    break;
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error(ex, "Error processing game state in GameLoopBackgroundService.");
                    // In case the connection was lost abruptly during read
                    if (!_readerService.IsConnected)
                    {
                        _dispatcherService.DispatchConnectionStatus(false);
                    }
                }

                try
                {
                    await Task.Delay(1000, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // Saída natural do loop ao dar Ctrl+C
                    break;
                }
            }
            
            Serilog.Log.Information("GameLoopBackgroundService shutting down gracefully.");
        }
    }
}
