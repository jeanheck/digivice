using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses
{
    public class ImportantItemsAddresses
    {
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long TreeBoots { get; set; }

        [JsonConverter(typeof(HexStringToLongConverter))]
        public long FishingPole { get; set; }

        [JsonConverter(typeof(HexStringToLongConverter))]
        public long AsukaTrophy { get; set; }
    }
}
