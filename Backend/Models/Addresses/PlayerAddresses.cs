namespace Backend.Models.Addresses
{
    public class PlayerAddresses
    {
        public string Name { get; set; } = string.Empty;
        public string Bits { get; set; } = string.Empty;
        public int NameBufferSize { get; set; }
        public string MapIdAddress { get; set; } = string.Empty;
    }
}
