using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses
{
    public class CardBattleAddresses
    {
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Id { get; set; }
    }
}
