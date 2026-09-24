namespace Backend.Domain.Models
{
    public record class NpcBattle
    {
        public string Id { get; set; } = string.Empty;
        public bool Won { get; set; }
    }
}
