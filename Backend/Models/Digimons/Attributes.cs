using System;

namespace Backend.Models.Digimons
{
    public class Attributes : IEquatable<Attributes>
    {
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Spirit { get; set; }
        public int Wisdom { get; set; }
        public int Speed { get; set; }
        public int Charisma { get; set; }

        public bool Equals(Attributes? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Strength == other.Strength && Defense == other.Defense && Spirit == other.Spirit && Wisdom == other.Wisdom && Speed == other.Speed && Charisma == other.Charisma;
        }

        public override bool Equals(object? obj) => Equals(obj as Attributes);
        public override int GetHashCode() => HashCode.Combine(Strength, Defense, Spirit, Wisdom, Speed, Charisma);
    }
}
