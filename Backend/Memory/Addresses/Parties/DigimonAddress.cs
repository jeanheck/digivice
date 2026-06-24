using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses.Parties
{
    public class DigimonAddress
    {
        public string Name { get; set; } = string.Empty;
        public int Id { get; set; }
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Address { get; set; }
    }
}
