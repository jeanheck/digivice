namespace Backend.Domain.Models
{
    public record class Player
    {
        public string Name { get; set; } = string.Empty;
        public int Bits { get; set; }
        public string MapId { get; set; } = string.Empty;
        public byte SeabedRoute { get; set; }
        public byte SeabedRouteType { get; set; }
    }
}
