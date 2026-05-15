using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Addresses
{
    public class PlayerAddresses
    {
        public int NameBufferSize { get; set; }

        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Name { get; set; }
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Bits { get; set; }
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long MapId { get; set; }
    }
}
