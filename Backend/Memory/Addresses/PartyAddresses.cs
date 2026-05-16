using System.Text.Json.Serialization;
using Backend.Memory.Converters;
using Backend.Memory.Addresses.Party;

namespace Backend.Memory.Addresses
{
    public class PartyAddresses
    {
        public List<SlotAddresses> Slots { get; set; } = [];
        public int BytesPerSlot { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int EmptySlotId { get; set; }
    }
}
