using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Addresses
{
    public class PartyAddresses
    {
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long PartySlot1 { get; set; }
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long PartySlot2 { get; set; }
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long PartySlot3 { get; set; }

        public int BytesPerSlot { get; set; }

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int EmptySlotId { get; set; }
    }
}
