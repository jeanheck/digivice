using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses.Parties.Digimons
{
    public class VitalAddresses
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Current { get; set; }

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Max { get; set; }
    }
}
