using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses
{
    public class DigimonBattleAddresses
    {
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Field { get; set; }
    }
}
