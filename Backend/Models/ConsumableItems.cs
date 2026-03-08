namespace Backend.Models
{
    public class ConsumableItems : IEquatable<ConsumableItems>
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

        public bool Equals(ConsumableItems? other)
        {
            if (other is null) return false;

            bool EqualOrNull<T>(T? a, T? b) where T : IEquatable<T>
            {
                if (a == null && b == null) return true;
                if (a == null || b == null) return false;
                return a.Equals(b);
            }

            return EqualOrNull(PowerCharge, other.PowerCharge) &&
                   EqualOrNull(SpiderWeb, other.SpiderWeb) &&
                   EqualOrNull(BambooSpear, other.BambooSpear);
        }

        public override bool Equals(object? obj) => Equals(obj as ConsumableItems);
        public override int GetHashCode() => HashCode.Combine(PowerCharge, SpiderWeb, BambooSpear);
    }
}
