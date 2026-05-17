using System.Collections.Generic;
using System.Text.Json.Serialization;
using Backend.Memory.Converters;
using Backend.Memory.Addresses.Party;

namespace Backend.Memory.Addresses.Digimon
{
    public class DigievolutionsAddresses
    {
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int ActiveDigievolution { get; set; }

        public List<SlotAddresses> Slots { get; set; } = [];

        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int UnlockedDigievolutionsStart { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int UnlockedDigievolutionEntryStride { get; set; }
        [JsonConverter(typeof(HexOrIntStringToIntConverter))]
        public int MaxUnlockedDigievolutions { get; set; }

        public void Deconstruct(
            out int unlockedDigievolutionsStart,
            out int unlockedDigievolutionEntryStride,
            out int maxUnlockedDigievolutions)
        {
            unlockedDigievolutionsStart = UnlockedDigievolutionsStart;
            unlockedDigievolutionEntryStride = UnlockedDigievolutionEntryStride;
            maxUnlockedDigievolutions = MaxUnlockedDigievolutions;
        }
    }
}
