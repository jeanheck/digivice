using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Models.Addresses
{
    public class ItemAddress
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Address { get; set; }
    }
}
