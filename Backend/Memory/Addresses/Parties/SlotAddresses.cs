using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses.Parties
{
    public class SlotAddresses
    {
        public int Index { get; set; }
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Address { get; set; }
    }
}
