using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Models.Quests
{
    public record class Requisite
    {
        public string Id { get; set; } = string.Empty;
        
        
        [JsonConverter(typeof(HexStringToLongConverter))]
        public long Address { get; set; }
        
        public bool IsDone { get; set; }
    }
}
