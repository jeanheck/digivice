using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Addresses.Digimon
{
    public class DigievolutionsAddresses
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int ActiveDigievolution { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Slot1 { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Slot2 { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Slot3 { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int UnlockedDigievolutionsStart { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int UnlockedDigievolutionEntryStride { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int MaxUnlockedDigievolutions { get; set; }
    }
}
