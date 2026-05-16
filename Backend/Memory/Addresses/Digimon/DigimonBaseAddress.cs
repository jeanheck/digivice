using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Memory.Addresses.Digimon
{
    public class DigimonBaseAddress
    {
        public string? Name { get; set; }
        public int Id { get; set; }
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Address { get; set; }
    }
}
