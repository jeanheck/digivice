namespace Backend.Models
{
    public record class ConsumableItems
    {
        public ConsumableItem? PowerCharge { get; set; }
        public ConsumableItem? SpiderWeb { get; set; }
        public ConsumableItem? BambooSpear { get; set; }

        public int GetQuantity(string id)
        {
            return id switch
            {
                "PowerCharge" => PowerCharge?.Quantity ?? 0,
                "SpiderWeb" => SpiderWeb?.Quantity ?? 0,
                "BambooSpear" => BambooSpear?.Quantity ?? 0,
                _ => 0
            };
        }
    }
}
