namespace Backend.Domain.Models
{
    public record class CardInventory
    {
        public Dictionary<int, int> Cards { get; init; } = [];

        public virtual bool Equals(CardInventory? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (Cards.Count != other.Cards.Count)
            {
                return false;
            }

            foreach (var (cardId, quantity) in Cards)
            {
                if (!other.Cards.TryGetValue(cardId, out var otherQuantity) || quantity != otherQuantity)
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var (cardId, quantity) in Cards)
            {
                hash.Add(cardId);
                hash.Add(quantity);
            }
            return hash.ToHashCode();
        }
    }
}
