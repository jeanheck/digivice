using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses
{
    public class PlayerAddresses
    {
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Bits { get; set; }
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long MapId { get; set; }
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long PreviousMapId { get; set; }
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long SeabedRoute { get; set; }
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long MapVariant { get; set; }
    }
}
