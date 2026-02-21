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
                Console.WriteLine($"Player name: {player.Name.PadRight(10)} | Bits: {player.Bits:N0}");
            }

            Console.WriteLine();

            if (party.Digimons.Count == 0)
            {
                Console.WriteLine("(No Digimons detected in party slots)");
            }
            else
            {
                for (int i = 0; i < party.Digimons.Count; i++)
                {
                    var d = party.Digimons[i];
                    Console.WriteLine($"Slot {i + 1}: {d.Name.PadRight(10)} [Lvl: {d.Level:D2}] [EXP: {d.Experience:D6}] HP: {d.CurrentHP:D3}/{d.MaxHP:D3} MP: {d.CurrentMP:D3}/{d.MaxMP:D3}");
                    Console.WriteLine($"   Status:   Atk:{d.Attack:D3} Def:{d.Defense:D3} Spt:{d.Spirit:D3} Wis:{d.Wisdom:D3} Spd:{d.Speed:D3} Cha:{d.Charisma:D3}");
                    Console.WriteLine($"   Resist:   Fir:{d.FireResistance:D3} Wat:{d.WaterResistance:D3} Ice:{d.IceResistance:D3} Wnd:{d.WindResistance:D3} Tdr:{d.ThunderResistance:D3} Mtl:{d.MetalResistance:D3} Drk:{d.DarkResistance:D3}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("\nMonitoring... (Press 'Q' to exit)");
        }
    }
}
