namespace Backend.Models.Digimons
{
    public class Technique
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        /// <summary>Physical, Magical, or Heal</summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>Fire, Water, Ice, Wind, Thunder, Dark, Machine, None, etc.</summary>
        public string Element { get; set; } = string.Empty;

        /// <summary>Numeric elemental power multiplier used by the game (16, 32, 48, 64, 72...)</summary>
        public int ElementStrength { get; set; }

        public int Mp { get; set; }
        public int Power { get; set; }
        public string Description { get; set; } = string.Empty;

        /// <summary>Level required to learn this technique for the associated Digivolution</summary>
        public int LearnLevel { get; set; }

        /// <summary>True if this is the Signature technique (highest LearnLevel in the set)</summary>
        public bool IsSignature { get; set; }
    }
}
