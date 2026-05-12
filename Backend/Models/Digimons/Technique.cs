namespace Backend.Models.Digimons
{
    public record class Technique
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Element { get; set; } = string.Empty;
        public int ElementStrength { get; set; }
        public int Mp { get; set; }
        public int Power { get; set; }
        public string Description { get; set; } = string.Empty;
        public int LearnLevel { get; set; }
        public bool IsSignature { get; set; }
    }
}
