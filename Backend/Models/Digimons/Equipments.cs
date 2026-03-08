namespace Backend.Models.Digimons
{
    public class Equipments : IEquatable<Equipments>
    {
        public int Head { get; set; }
        public int Body { get; set; }
        public int RightHand { get; set; }
        public int LeftHand { get; set; }
        public int Accessory1 { get; set; }
        public int Accessory2 { get; set; }

        public bool Equals(Equipments? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Head == other.Head && Body == other.Body && RightHand == other.RightHand && LeftHand == other.LeftHand && Accessory1 == other.Accessory1 && Accessory2 == other.Accessory2;
        }

        public override bool Equals(object? obj) => Equals(obj as Equipments);
        public override int GetHashCode() => HashCode.Combine(Head, Body, RightHand, LeftHand, Accessory1, Accessory2);
    }
}
