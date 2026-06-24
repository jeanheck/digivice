namespace Backend.Domain.Models
{
    public record class Auction
    {
        public string Id { get; set; } = string.Empty;

        public byte Value { get; set; }
    }
}
