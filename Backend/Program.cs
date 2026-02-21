using Backend.Interfaces;
using Backend.Services;
using Serilog;

namespace Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Initializing Digivice...");

            var processService = new WindowsProcessService();
            var memoryProvider = new WindowsMemoryProvider();

            using (IMemoryReader reader = new MemoryReader(processService, memoryProvider))
            {
                if (!reader.TryConnect())
                {
                    Log.Error("Failed to connect to DuckStation. Make sure the emulator and game are open.");
                    return;
                }

                var gameState = new GameStateService(reader);

                while (true)
                {
                    try { Console.Clear(); } catch { }
                    Console.WriteLine("                    DIGIVICE                    ");
                    Console.WriteLine();

                    var player = gameState.GetPlayer();
                    if (player != null)
                    {
                        Console.WriteLine($"Player name: {player.Name}");
                    }
                    Console.WriteLine("Party:");

                    var party = gameState.GetParty();
                    if (party.Digimons.Count == 0)
                    {
                        Console.WriteLine("(No Digimons detected in party slots)");
                    }
                    else
                    {
                        for (int i = 0; i < party.Digimons.Count; i++)
                        {
                            var digi = party.Digimons[i];
                            Console.WriteLine($" - Slot {i + 1}: {digi.Name.PadRight(10)} [ID: {digi.Id}] (Base: 0x{digi.BaseAddress:X8})");
                        }
                    }

                    Console.WriteLine("-------------------------------------------------");
                    Console.WriteLine("\nMonitoring... (Press 'Q' to exit)");

                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q) break;
                    Thread.Sleep(1000);
                }
            }

            Log.Information("Application ended.");
        }
    }
}
