using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Models.Addresses
{
    public class PlayerAddresses
    {
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Name { get; set; }
        
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Bits { get; set; }
        
        public int NameBufferSize { get; set; }
        
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long MapIdAddress { get; set; }
    }
}
