namespace Backend.Models.Quests
{
    public class QuestStep
    {
        public int Number { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        /// <summary>
        /// Optional informational prerequisites for this step.
        /// Displayed as a checklist inside the step — does NOT block progress.
        /// </summary>
        public List<Requisite>? Prerequisites { get; set; }

        public string? Address { get; set; }

        /// <summary>
        /// Bitmask to check against the byte at Address.
        /// Step is completed if (byte AND BitMask) != 0.
        /// 
        /// For cumulative flags: use a single bit (e.g., "0x04" for bit 2).
        /// For rotating/phase flags: use multiple bits (e.g., "0xE0" for bits 5+6+7,
        ///   meaning "step is done if ANY of these bits is set").
        /// 
        /// If null, uses legacy behavior: byte == 1 means completed.
        /// </summary>
        public string? BitMask { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not QuestStep other) return false;

            bool prereqsEqual = (Prerequisites == null && other.Prerequisites == null) ||
                                (Prerequisites != null && other.Prerequisites != null &&
                                 Prerequisites.Count == other.Prerequisites.Count &&
                                 Prerequisites.SequenceEqual(other.Prerequisites));

            return Number == other.Number &&
                   Description == other.Description &&
                   IsCompleted == other.IsCompleted &&
                   Address == other.Address &&
                   BitMask == other.BitMask &&
                   prereqsEqual;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Number);
            hash.Add(Description);
            hash.Add(IsCompleted);
            hash.Add(Address);
            hash.Add(BitMask);
            if (Prerequisites != null)
            {
                foreach (var p in Prerequisites) hash.Add(p);
            }
            return hash.ToHashCode();
        }
    }
}
