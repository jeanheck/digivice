using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses.Digimon
{
    public class AttributesAddresses
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Strength { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Defense { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Spirit { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Wisdow { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Speed { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Charisma { get; set; }
    }
}
