using System;
using Backend.Models;

namespace Backend.UI
{
    public class ConsoleRenderer
    {
        public void Render(Player? player, Party party)
        {
            try { Console.Clear(); } catch { }

            Console.WriteLine("                    DIGIVICE                    ");
            Console.WriteLine();

            if (player != null)
            {
                Console.WriteLine($"Player name: {player.Name}");
            }

            Console.WriteLine("Party:");

            if (party.Digimons.Count == 0)
            {
                Console.WriteLine("(No Digimons detected in party slots)");
            }
            else
            {
                for (int i = 0; i < party.Digimons.Count; i++)
                {
                    var digi = party.Digimons[i];
                    Console.WriteLine($" - Slot {i + 1}: {digi.Name.PadRight(10)} [Lvl: {digi.Level:D2}] [EXP: {digi.Experience:D6}]");
                }
            }

            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("\nMonitoring... (Press 'Q' to exit)");
        }
    }
}
