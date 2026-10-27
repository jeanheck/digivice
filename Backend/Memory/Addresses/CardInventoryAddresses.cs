using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses
{
    public class CardInventoryAddresses
    {
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Start { get; set; }

        public int TotalCards { get; set; }
    }
}
