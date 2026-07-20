namespace Backend.Domain.Models
{
    public record class Player
    {
        public int Bits { get; set; }
        public string MapId { get; set; } = string.Empty;
        public string PreviousMapId { get; set; } = string.Empty;
        public byte SeabedRoute { get; set; }
        public byte MapVariant { get; set; }
    }
}
