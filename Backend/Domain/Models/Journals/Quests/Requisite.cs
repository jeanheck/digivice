namespace Backend.Domain.Models.Journals.Quests
{
    public record class Requisite
    {
        public string Id { get; set; } = string.Empty;
        public byte Value { get; set; }
    }
}
