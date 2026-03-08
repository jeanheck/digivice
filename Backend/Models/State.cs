namespace Backend.Models
{
    public class State
    {
        public Player? Player { get; set; }
        public Party? Party { get; set; }
        public ImportantItems? ImportantItems { get; set; }
        public ConsumableItems? ConsumableItems { get; set; }
        public Journal? Journal { get; set; }
    }
}
