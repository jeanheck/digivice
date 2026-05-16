namespace Backend.Domain.Models
{
    public record class State
    {
        public Player? Player { get; set; }
        public Party? Party { get; set; }
        public Journal? Journal { get; set; }
    }
}
