using System.Text.Json.Serialization;
using Backend.Memory.Converters;

namespace Backend.Memory.Addresses.Journals.Quests
{
    public class RequisiteAddresses
    {
        public string Id { get; set; } = string.Empty;

        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Address { get; set; }
    }
}
