using Backend.Events.Interfaces;
using Backend.Interfaces;
using Backend.Services;

namespace Backend.Diagnostics
{
    public class DebugMonitor
    {
        private readonly IProcessService _processService;
        private readonly IMemoryProvider _memoryProvider;
        private readonly DebugConsoleRenderer _debugConsoleRenderer;
        private readonly IEventDispatcherService _dispatcherService;
        private readonly IMemoryReaderService _readerService;
        private readonly GameStateService _gameStateService;

        public DebugMonitor(
            IProcessService processService,
            IMemoryProvider memoryProvider,
            DebugConsoleRenderer renderer,
            IEventDispatcherService dispatcherService,
            IMemoryReaderService readerService,
            GameStateService gameStateService)
        {
            _processService = processService;
            _memoryProvider = memoryProvider;
            _debugConsoleRenderer = renderer;
            _dispatcherService = dispatcherService;
            _readerService = readerService;
            _gameStateService = gameStateService;
        }

        public void Run()
        {
            if (!_readerService.TryConnect())
            {
                Serilog.Log.Error("Failed to connect to DuckStation. Make sure the emulator and game are open.");
                _dispatcherService.DispatchConnectionStatus(false);
                return;
            }

            _dispatcherService.DispatchConnectionStatus(true);

            while (true)
            {
                if (!_readerService.IsConnected)
                {
                    Serilog.Log.Error("Connection to DuckStation lost. Closing AppMonitor.");
                    _dispatcherService.DispatchConnectionStatus(false);
                    break;
                }

                var state = _gameStateService.GetState();

                if (state?.Player != null)
                {
                    // Dispatch any state differences
                    _dispatcherService.ProcessGameState(state);

                    // Só tenta renderizar UI no console e ler teclas se existir um console real anexado
                    if (!Console.IsOutputRedirected && !Console.IsInputRedirected)
                    {
                        _debugConsoleRenderer.Render(state);
                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q) break;
                    }
                }

                Thread.Sleep(1000);
            }
        }
    }
}
