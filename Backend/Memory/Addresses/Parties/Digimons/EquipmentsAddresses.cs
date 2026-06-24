using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses.Parties.Digimons
{
    public class EquipmentsAddresses
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Head { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Body { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Right { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Left { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Accessory1 { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Accessory2 { get; set; }
    }
}
