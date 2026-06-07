using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses
{
    public class AuctionEntryAddresses
    {
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long BitMask { get; set; }
    }
}
