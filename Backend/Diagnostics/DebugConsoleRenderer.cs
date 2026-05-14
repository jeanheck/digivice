using System.Text;
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

        // ANSI Color Codes
        private const string Reset = "\x1b[0m";
        private const string Cyan = "\x1b[36m";
        private const string Yellow = "\x1b[33m";
        private const string Green = "\x1b[32m";
        private const string Red = "\x1b[31m";
        private const string Blue = "\x1b[34m";
        private const string Gray = "\x1b[90m";
        private bool _firstRender = true;

        public void Render(State? state)
        {
            var sb = new StringBuilder();

            if (_firstRender)
            {
                Console.Clear();
                _firstRender = false;
            }

            // Stabilize cursor instead of clearing repeatedly
            Console.SetCursorPosition(0, 0);

            RenderHeader(sb, state);

            if (state?.Player == null || state?.Party == null)
            {
                sb.AppendLine($"{Red}WAITING FOR GAME PROCESS DATA...{Reset}");
                Console.Write(sb.ToString());
                return;
            }

            RenderPlayer(sb, state.Player);
            RenderParty(sb, state.Party);

            sb.AppendLine($"{Gray}-------------------------------------------------{Reset}");
            sb.AppendLine($"\nMonitoring... (Press 'Ctrl + C' to exit)");

            // Print the entire buffer at once to minimize flickering
            Console.Write(sb.ToString());
        }

        private void RenderHeader(StringBuilder sb, State? state)
        {
            sb.AppendLine($"{Cyan}                    DIGIVICE (MONITOR)                    {Reset}");
            string status = state != null ? $"{Green}RUNNING{Reset}" : $"{Red}WAITING{Reset}";
            sb.AppendLine($"Status: {status} | {DateTime.Now:HH:mm:ss}");
            sb.AppendLine();
        }

        private void RenderPlayer(StringBuilder sb, Player player)
        {
            sb.AppendLine($"{Cyan}PLAYER:{Reset} {player.Name.PadRight(NamePadding)} | {Yellow}BITS:{Reset} {player.Bits?.ToString(BitsFormat) ?? "Unknown"}");
            sb.AppendLine();
        }

        private void RenderParty(StringBuilder sb, Party party)
        {
            var activeSlots = party.Slots.Where(d => d != null).ToList();

            if (activeSlots.Count == 0)
            {
                sb.AppendLine($"{Gray}(No Digimons detected in party slots){Reset}");
                return;
            }

            foreach (var d in activeSlots)
            {
                RenderDigimon(sb, d!);
            }
        }

        private void RenderDigimon(StringBuilder sb, Digimon d)
        {
            var b = d.BasicInfo;
            sb.AppendLine($"{Yellow}Slot {d.SlotIndex}:{Reset} {Cyan}{b.Name.PadRight(NamePadding)}{Reset} [Lv.{b.Level.ToString(LvlFormat)}] [EXP:{b.Experience.ToString(ExpFormat)}]");

            // HP Bar
            sb.Append("   HP: ");
            AppendProgressBar(sb, b.CurrentHP, b.MaxHP, GetHpColor(b.CurrentHP, b.MaxHP));
            sb.AppendLine($" {b.CurrentHP.ToString(StatFormat)}/{b.MaxHP.ToString(StatFormat)}");

            // MP Bar
            sb.Append("   MP: ");
            AppendProgressBar(sb, b.CurrentMP, b.MaxMP, Blue);
            sb.AppendLine($" {b.CurrentMP.ToString(StatFormat)}/{b.MaxMP.ToString(StatFormat)}");

            // Attributes
            var attr = d.Attributes;
            sb.AppendLine($"{Gray}   Stats:   {Reset}Atk:{attr.Strength.ToString(StatFormat)} Def:{attr.Defense.ToString(StatFormat)} Spt:{attr.Spirit.ToString(StatFormat)} Wis:{attr.Wisdom.ToString(StatFormat)} Spd:{attr.Speed.ToString(StatFormat)} Cha:{attr.Charisma.ToString(StatFormat)}");

            // Resistances
            var res = d.Resistances;
            sb.AppendLine($"{Gray}   Resist:  {Reset}Fir:{res.Fire.ToString(StatFormat)} Wat:{res.Water.ToString(StatFormat)} Ice:{res.Ice.ToString(StatFormat)} Wnd:{res.Wind.ToString(StatFormat)} Tdr:{res.Thunder.ToString(StatFormat)} Mtl:{res.Machine.ToString(StatFormat)} Drk:{res.Dark.ToString(StatFormat)}");

            // Equipments
            var eq = d.Equipments;
            sb.AppendLine($"{Gray}   Equips:  {Reset}H:{eq.Head} B:{eq.Body} R:{eq.RightHand} L:{eq.LeftHand} A1:{eq.Accessory1} A2:{eq.Accessory2}");

            // Evolutions
            sb.Append($"{Gray}   Evos:    {Reset}");
            for (int i = 0; i < 3; i++)
            {
                var evo = d.Digievolutions[i];
                string evoStr = evo != null ? $"{Yellow}[{evo.Id}Lv{evo.Level}]{Reset}" : $"{Gray}[Empty]{Reset}";
                sb.Append($"S{i + 1}:{evoStr} ");
            }
            sb.AppendLine($" | {Yellow}ActiveEvo:{Reset} {d.ActiveDigievolutionId?.ToString() ?? "NULL"}");
            sb.AppendLine();
        }

        private void AppendProgressBar(StringBuilder sb, int current, int max, string color)
        {
            const int barLength = 10;
            int filled = max > 0 ? (int)((float)current / max * barLength) : 0;
            filled = Math.Clamp(filled, 0, barLength);

            sb.Append("[");
            sb.Append(color);
            sb.Append(new string('█', filled));
            sb.Append(new string('-', barLength - filled));
            sb.Append(Reset);
            sb.Append("]");
        }

        private string GetHpColor(int current, int max)
        {
            if (max == 0) return Red;
            float percent = (float)current / max;
            if (percent > 0.5f) return Green;
            if (percent > 0.2f) return Yellow;
            return Red;
        }
    }
}
