using System.Text.Json.Serialization;
using Backend.Memory.Addresses.Parties.Digimons;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses
{
    public class EnemyAddresses
    {
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long EnemySlotBase { get; set; }

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int SlotStride { get; set; }

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int SlotCount { get; set; }

        [JsonConverter(typeof(HexStringToLongConverter))]
        public long ActiveUnitId { get; set; }

        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Field { get; set; }

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Id { get; set; }

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Condition { get; set; }

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Strength { get; set; }

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Defense { get; set; }

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Speed { get; set; }

        public VitalAddresses HP { get; set; } = new();
    }
}
