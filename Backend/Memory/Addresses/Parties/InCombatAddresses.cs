using System.Text.Json.Serialization;
using Backend.Memory.Addresses.Parties.Digimons;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses.Parties
{
    public class InCombatAddresses
    {
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long AllySlotBase { get; set; }

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int SlotStride { get; set; }

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Id { get; set; }

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Condition { get; set; }

        public VitalAddresses HP { get; set; } = new();

        public VitalAddresses MP { get; set; } = new();
    }
}
