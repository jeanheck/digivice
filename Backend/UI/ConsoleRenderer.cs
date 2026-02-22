using System;
using Backend.Models;
using Backend.Models.Digimons;

namespace Backend.UI
{
    public class ConsoleRenderer
    {
        private const int NamePadding = 10;
        private const string StatFormat = "D3";
        private const string ExpFormat = "D6";
        private const string LvlFormat = "D2";
        private const string BitsFormat = "N0";

        public void Render(Player? player)
        {
            try { Console.Clear(); } catch { }

            Console.WriteLine("                    DIGIVICE                    ");
            Console.WriteLine();

            if (player == null) return;

            Console.WriteLine($"Player name: {player.Name.PadRight(NamePadding)} | Bits: {player.Bits.ToString(BitsFormat)}");
            Console.WriteLine();

            if (player.Party.Digimons.Count == 0)
            {
                Console.WriteLine("(No Digimons detected in party slots)");
            }
            else
            {
                for (int i = 0; i < player.Party.Digimons.Count; i++)
                {
                    var d = player.Party.Digimons[i];
                    var b = d.BasicInfo;
                    Console.WriteLine($"Slot {d.SlotIndex}: {b.Name.PadRight(NamePadding)} [Lvl: {b.Level.ToString(LvlFormat)}] [EXP: {b.Experience.ToString(ExpFormat)}] HP: {b.CurrentHP.ToString(StatFormat)}/{b.MaxHP.ToString(StatFormat)} MP: {b.CurrentMP.ToString(StatFormat)}/{b.MaxMP.ToString(StatFormat)}");
                    Console.WriteLine($"   Status:   Atk:{d.Attributes.Attack.ToString(StatFormat)} Def:{d.Attributes.Defense.ToString(StatFormat)} Spt:{d.Attributes.Spirit.ToString(StatFormat)} Wis:{d.Attributes.Wisdom.ToString(StatFormat)} Spd:{d.Attributes.Speed.ToString(StatFormat)} Cha:{d.Attributes.Charisma.ToString(StatFormat)}");
                    Console.WriteLine($"   Resist:   Fir:{d.Resistances.Fire.ToString(StatFormat)} Wat:{d.Resistances.Water.ToString(StatFormat)} Ice:{d.Resistances.Ice.ToString(StatFormat)} Wnd:{d.Resistances.Wind.ToString(StatFormat)} Tdr:{d.Resistances.Thunder.ToString(StatFormat)} Mtl:{d.Resistances.Metal.ToString(StatFormat)} Drk:{d.Resistances.Dark.ToString(StatFormat)}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("\nMonitoring... (Press 'Q' to exit)");
        }
    }
}
