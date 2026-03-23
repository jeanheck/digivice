namespace Backend.Models.Quests
{
    public class MapCoordinates
    {
        public float X { get; set; }
        public float Y { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not MapCoordinates other) return false;
            return Math.Abs(X - other.X) < 0.001f && Math.Abs(Y - other.Y) < 0.001f;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
