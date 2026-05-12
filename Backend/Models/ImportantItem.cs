namespace Backend.Models
{
    public record class ImportantItem : IItem
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool Has { get; set; }
    }
}
