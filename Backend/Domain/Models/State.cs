namespace Backend.Domain.Models
{
    public record class State
    {
        public Player Player { get; set; } = new();
        public Party Party { get; set; } = new();
        public Journal Journal { get; set; } = new();
    }
}
