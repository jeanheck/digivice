using System;
using System.Threading;
using Backend.Events.Interfaces;
using Backend.Interfaces;
using Backend.UI;
using Backend.Services;

namespace Backend.Core
{
    public class Monitor
    {
        private readonly IProcessService _processService;
        private readonly IMemoryProvider _memoryProvider;
        private readonly ConsoleRenderer _renderer;
        private readonly IEventDispatcherService _dispatcherService;

        public Monitor(IProcessService processService, IMemoryProvider memoryProvider, ConsoleRenderer renderer, IEventDispatcherService dispatcherService)
        {
            _processService = processService;
            _memoryProvider = memoryProvider;
            _renderer = renderer;
            _dispatcherService = dispatcherService;
        }

        public void Run()
        {
            using (MemoryReaderService reader = new MemoryReaderService(_processService, _memoryProvider))
            {
                if (!reader.TryConnect())
                {
                    Serilog.Log.Error("Failed to connect to DuckStation. Make sure the emulator and game are open.");
                    _dispatcherService.DispatchConnectionStatus(false);
                    return;
                }

                _dispatcherService.DispatchConnectionStatus(true);
                var gameState = new GameStateService(reader);

                while (true)
                {
                    if (!reader.IsConnected)
                    {
                        Serilog.Log.Error("Connection to DuckStation lost. Closing AppMonitor.");
                        _dispatcherService.DispatchConnectionStatus(false);
                        break;
                    }

                    var player = gameState.GetPlayer();

                    if (player != null)
                    {
                        // Dispatch any state differences
                        _dispatcherService.ProcessGameState(player);
                        _renderer.Render(player);
                    }

                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q) break;
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
