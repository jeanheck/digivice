using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Models.Addresses
{
    public class ItemAddress
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Address { get; set; }
    }
}
