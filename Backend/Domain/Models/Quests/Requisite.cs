namespace Backend.Domain.Backend.Domain.Models.Quests
{
    public record class Requisite
    {
        public string Id { get; set; } = string.Empty;
        public byte Value { get; set; }
    }
}
