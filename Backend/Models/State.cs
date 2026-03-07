namespace Backend.Models
{
    public class State
    {
        public Player? Player { get; set; }
        public Party? Party { get; set; }
        public Dictionary<string, bool> ImportantItems { get; set; } = new();
        public Journal? Journal { get; set; }
    }
}
