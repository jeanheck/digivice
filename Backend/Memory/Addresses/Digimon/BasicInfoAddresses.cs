using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Memory.Addresses.Digimon
{
    public class BasicInfoAddresses
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Experience { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Level { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int CurrentHP { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int MaxHP { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int CurrentMP { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int MaxMP { get; set; }
    }
}
