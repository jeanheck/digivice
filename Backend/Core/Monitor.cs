using System;
using System.Threading;
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

        public Monitor(IProcessService processService, IMemoryProvider memoryProvider, ConsoleRenderer renderer)
        {
            _processService = processService;
            _memoryProvider = memoryProvider;
            _renderer = renderer;
        }

        public void Run()
        {
            using (MemoryReaderService reader = new MemoryReaderService(_processService, _memoryProvider))
            {
                if (!reader.TryConnect())
                {
                    Serilog.Log.Error("Failed to connect to DuckStation. Make sure the emulator and game are open.");
                    return;
                }

                var gameState = new GameStateService(reader);

                while (true)
                {
                    if (!reader.IsConnected)
                    {
                        Serilog.Log.Error("Connection to DuckStation lost. Closing AppMonitor.");
                        break;
                    }

                    var player = gameState.GetPlayer();

                    _renderer.Render(player);

                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q) break;
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
