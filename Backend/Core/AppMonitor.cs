using System;
using System.Threading;
using Backend.Interfaces;
using Backend.UI;
using Backend.Services;

namespace Backend.Core
{
    public class AppMonitor
    {
        private readonly IProcessService _processService;
        private readonly IMemoryProvider _memoryProvider;
        private readonly ConsoleRenderer _renderer;

        public AppMonitor(IProcessService processService, IMemoryProvider memoryProvider, ConsoleRenderer renderer)
        {
            _processService = processService;
            _memoryProvider = memoryProvider;
            _renderer = renderer;
        }

        public void Run()
        {
            using (IMemoryReader reader = new MemoryReader(_processService, _memoryProvider))
            {
                if (!reader.TryConnect())
                {
                    Serilog.Log.Error("Failed to connect to DuckStation. Make sure the emulator and game are open.");
                    return;
                }

                var gameState = new GameStateService(reader);

                while (true)
                {
                    var player = gameState.GetPlayer();
                    var party = gameState.GetParty();

                    _renderer.Render(player, party);

                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q) break;
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
