using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Models.Quests
{
    public record class QuestStep
    {
        public int Number { get; set; }
        public bool IsCompleted { get; set; }
        
        /// <summary>
        /// Optional informational requisites for this step.
        /// Displayed as a checklist inside the step — does NOT block progress.
        /// </summary>
        public List<Requisite>? Requisites { get; set; }

        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Address { get; set; }

        /// <summary>
        /// Bitmask to check against the byte at Address.
        /// Step is completed if (byte AND BitMask) != 0.
        /// </summary>
        public string? BitMask { get; set; }

        public virtual bool Equals(QuestStep? other)
        {
            if (other is null) return false;

            bool requisitesEqual = (Requisites == null && other.Requisites == null) ||
                                 (Requisites != null && other.Requisites != null &&
                                  Requisites.SequenceEqual(other.Requisites));

            return Number == other.Number &&
                   IsCompleted == other.IsCompleted &&
                   Address == other.Address &&
                   BitMask == other.BitMask &&
                   requisitesEqual;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Number);
            hash.Add(IsCompleted);
            hash.Add(Address);
            hash.Add(BitMask);
            
            if (Requisites != null)
            {
                foreach (var r in Requisites) hash.Add(r);
            }
            return hash.ToHashCode();
        }
    }
}
