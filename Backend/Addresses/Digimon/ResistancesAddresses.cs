using System.Text.Json.Serialization;
using Backend.Utils;

namespace Backend.Addresses.Digimon
{
    public class ResistancesAddresses
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Fire { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Water { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Ice { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Wind { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Thunder { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Machine { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int Dark { get; set; }
    }
}
