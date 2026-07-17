using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses
{
    public class AuctionAddresses
    {
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Address { get; set; }

        [JsonConverter(typeof(HexStringToLongConverter))]
        public long BitMask { get; set; }
    }
}
