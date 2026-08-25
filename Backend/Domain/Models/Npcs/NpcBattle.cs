namespace Backend.Domain.Models.Npcs
{
    public record class NpcBattle
    {
        public string Id { get; set; } = string.Empty;
        public byte Value { get; set; }
    }
}
