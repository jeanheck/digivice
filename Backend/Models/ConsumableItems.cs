namespace Backend.Models
{
    public record class ConsumableItems
    {
        public ConsumableItem PowerCharge { get; set; }
        public ConsumableItem SpiderWeb { get; set; }
        public ConsumableItem BambooSpear { get; set; }

        public int GetQuantity(string id)
        {
            return id switch
            {
                "PowerCharge" => PowerCharge.Quantity,
                "SpiderWeb" => SpiderWeb.Quantity,
                "BambooSpear" => BambooSpear.Quantity,
                _ => 0
            };
        }
    }
}
