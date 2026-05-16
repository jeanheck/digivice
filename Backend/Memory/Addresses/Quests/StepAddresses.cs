using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Memory.Addresses.Quests
{
    public class StepAddresses
    {
        public int Number { get; set; }

        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Address { get; set; }

        [JsonConverter(typeof(HexStringToLongConverter))]
        public long? BitMask { get; set; }

        public List<RequisiteAddresses>? Requisites { get; set; }
    }
}
