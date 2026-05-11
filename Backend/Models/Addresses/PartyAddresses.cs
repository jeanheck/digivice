using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Models.Addresses
{
    public class PartyAddresses
    {
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long PartySlot1 { get; set; }
        
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long PartySlot2 { get; set; }
        
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long PartySlot3 { get; set; }
        public int PartySlotStride { get; set; }
    }
}
