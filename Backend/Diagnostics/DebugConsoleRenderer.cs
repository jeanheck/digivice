using Backend.Models;

namespace Backend.Diagnostics
{
    public class DebugConsoleRenderer
    {
        private const int NamePadding = 10;
        private const string StatFormat = "D3";
        private const string ExpFormat = "D6";
        private const string LvlFormat = "D2";
        private const string BitsFormat = "N0";

        public void Render(State? state)
        {
            try { Console.Clear(); } catch { }

            Console.WriteLine("                    DIGIVICE                    ");
            Console.WriteLine();

            if (state?.Player == null || state?.Party == null) return;

            Console.WriteLine($"Player name: {state.Player.Name.PadRight(NamePadding)} | Bits: {state.Player.Bits.ToString(BitsFormat)}");
            Console.WriteLine();

            var activeSlots = state.Party.Slots.Where(d => d != null).ToList();

            if (activeSlots.Count == 0)
            {
                Console.WriteLine("(No Digimons detected in party slots)");
            }
            else
            {
                for (int i = 0; i < activeSlots.Count; i++)
                {
                    var d = activeSlots[i]!;
                    var b = d.BasicInfo;
                    Console.WriteLine($"Slot {d.SlotIndex}: {b.Name.PadRight(NamePadding)} [Lvl: {b.Level.ToString(LvlFormat)}] [EXP: {b.Experience.ToString(ExpFormat)}] HP: {b.CurrentHP.ToString(StatFormat)}/{b.MaxHP.ToString(StatFormat)} MP: {b.CurrentMP.ToString(StatFormat)}/{b.MaxMP.ToString(StatFormat)}");
                    Console.WriteLine($"   Status:   Atk:{d.Attributes.Strength.ToString(StatFormat)} Def:{d.Attributes.Defense.ToString(StatFormat)} Spt:{d.Attributes.Spirit.ToString(StatFormat)} Wis:{d.Attributes.Wisdom.ToString(StatFormat)} Spd:{d.Attributes.Speed.ToString(StatFormat)} Cha:{d.Attributes.Charisma.ToString(StatFormat)}");
                    Console.WriteLine($"   Resist:   Fir:{d.Resistances.Fire.ToString(StatFormat)} Wat:{d.Resistances.Water.ToString(StatFormat)} Ice:{d.Resistances.Ice.ToString(StatFormat)} Wnd:{d.Resistances.Wind.ToString(StatFormat)} Tdr:{d.Resistances.Thunder.ToString(StatFormat)} Mtl:{d.Resistances.Machine.ToString(StatFormat)} Drk:{d.Resistances.Dark.ToString(StatFormat)}");

                    var eq = d.Equipments;
                    Console.WriteLine($"   Equips:   H:{eq.Head} B:{eq.Body} R:{eq.RightHand} L:{eq.LeftHand} A1:{eq.Accessory1} A2:{eq.Accessory2}");

                    var evos = d.EquippedDigievolutions;
                    string evoStr1 = evos[0] != null ? $"[{evos[0]!.Id}Lv{evos[0]!.Level}]" : "[Empty]";
                    string evoStr2 = evos[1] != null ? $"[{evos[1]!.Id}Lv{evos[1]!.Level}]" : "[Empty]";
                    string evoStr3 = evos[2] != null ? $"[{evos[2]!.Id}Lv{evos[2]!.Level}]" : "[Empty]";
                    Console.WriteLine($"   Evos:     S1:{evoStr1} S2:{evoStr2} S3:{evoStr3}  | ActiveEvoId: {d.ActiveDigievolutionId?.ToString() ?? "NULL"}");

                    Console.WriteLine();
                }
            }

            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("\nMonitoring... (Press 'Q' to exit)");
        }
    }
}
