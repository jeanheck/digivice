using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses.Party.Digimon
{
    public class VitalsAddresses
    {
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
