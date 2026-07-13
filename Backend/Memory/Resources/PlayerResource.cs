namespace Backend.Memory.Resources
{
    public class PlayerResource
    {
        public byte[]? NameInBytes { get; set; }
        public int? Bits { get; set; }
        public short? MapId { get; set; }
        public short? PreviousMapId { get; set; }
        public byte? SeabedRoute { get; set; }
        public byte? SeabedRouteType { get; set; }
    }
}
