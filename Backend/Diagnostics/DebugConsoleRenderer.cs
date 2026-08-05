using System.Text;
using Backend.Domain.Models;
using Backend.Domain.Models.Parties;

namespace Backend.Diagnostics
{
    public class DebugConsoleRenderer
    {
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
        private bool FirstRender = true;

        public void Render(State? state)
        {
            var sb = new StringBuilder();

            if (FirstRender)
            {
                Console.Clear();
                FirstRender = false;
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
            sb.AppendLine($"{Cyan}PLAYER:{Reset} {Yellow}BITS:{Reset} {player.Bits.ToString(BitsFormat)}");
            sb.AppendLine();
        }

        private void RenderParty(StringBuilder sb, Party party)
        {
            var activeSlots = party.Slots
                .Where(s => s.Digimon != null)
                .ToList();

            if (activeSlots.Count == 0)
            {
                sb.AppendLine($"{Gray}(No Digimons detected in party slots){Reset}");
                return;
            }

            foreach (var slot in activeSlots)
            {
                RenderDigimon(sb, slot);
            }
        }

        private void RenderDigimon(StringBuilder sb, DigimonSlot slot)
        {
            var digimon = slot.Digimon!;
            var hp = digimon.HP;
            var mp = digimon.MP;
            sb.AppendLine($"{Yellow}Slot {slot.Index} (ID: {slot.DigimonId}):{Reset} [Lv.{digimon.Level.ToString(LvlFormat)}] [TP:{digimon.TP.ToString(StatFormat)}] [EXP:{digimon.Experience.ToString(ExpFormat)}]");

            // HP Bar
            sb.Append("   HP: ");
            AppendProgressBar(sb, hp.Current, hp.Max, GetHpColor(hp.Current, hp.Max));
            sb.AppendLine($" {hp.Current.ToString(StatFormat)}/{hp.Max.ToString(StatFormat)}");

            // MP Bar
            sb.Append("   MP: ");
            AppendProgressBar(sb, mp.Current, mp.Max, Blue);
            sb.AppendLine($" {mp.Current.ToString(StatFormat)}/{mp.Max.ToString(StatFormat)}");

            var inCombat = digimon.InCombat;
            sb.AppendLine($"   InCombat Condition: {inCombat.Condition} | HP: {inCombat.HP.Current.ToString(StatFormat)}/{inCombat.HP.Max.ToString(StatFormat)} | MP: {inCombat.MP.Current.ToString(StatFormat)}/{inCombat.MP.Max.ToString(StatFormat)}");

            // Attributes
            var attributes = digimon.Attributes;
            sb.AppendLine($"{Gray}   Stats:   {Reset}Atk:{attributes.Strength.ToString(StatFormat)} Def:{attributes.Defense.ToString(StatFormat)} Spt:{attributes.Spirit.ToString(StatFormat)} Wis:{attributes.Wisdom.ToString(StatFormat)} Spd:{attributes.Speed.ToString(StatFormat)} Cha:{attributes.Charisma.ToString(StatFormat)}");

            // Resistances
            var resistances = digimon.Resistances;
            sb.AppendLine($"{Gray}   Resist:  {Reset}Fir:{resistances.Fire.ToString(StatFormat)} Wat:{resistances.Water.ToString(StatFormat)} Ice:{resistances.Ice.ToString(StatFormat)} Wnd:{resistances.Wind.ToString(StatFormat)} Tdr:{resistances.Thunder.ToString(StatFormat)} Mtl:{resistances.Machine.ToString(StatFormat)} Drk:{resistances.Dark.ToString(StatFormat)}");

            // Equipments
            var equipments = digimon.Equipments;
            sb.AppendLine($"{Gray}   Equips:  {Reset}H:{equipments.Head} B:{equipments.Body} R:{equipments.Right} L:{equipments.Left} A1:{equipments.Accessory1} A2:{equipments.Accessory2}");

            // Evolutions
            sb.Append($"{Gray}   Evos:    {Reset}");
            for (int i = 0; i < 3; i++)
            {
                var evolution = digimon.Digievolutions.FirstOrDefault(e => e.Index == (i + 1));
                string evolutionStr = evolution != null && evolution.DigievolutionId != null && evolution.Digievolution != null
                    ? $"{Yellow}[{evolution.DigievolutionId}Lv{evolution.Digievolution.Level}]{Reset}"
                    : $"{Gray}[Empty]{Reset}";
                sb.Append($"S{i + 1}:{evolutionStr} ");
            }
            sb.AppendLine($" | {Yellow}ActiveEvo:{Reset} {digimon.ActiveDigievolutionId.ToString()}");
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
