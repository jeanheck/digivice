using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Addresses.Digimon
{
    public class EquipmentsAddresses
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Head { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Body { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int RightHand { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int LeftHand { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Accessory1 { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Accessory2 { get; set; }
    }
}
