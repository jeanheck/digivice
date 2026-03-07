using System;

namespace Backend.Models.Digimons
{
    public class Resistances : IEquatable<Resistances>
    {
        public int Fire { get; set; }
        public int Water { get; set; }
        public int Ice { get; set; }
        public int Wind { get; set; }
        public int Thunder { get; set; }
        public int Machine { get; set; }
        public int Dark { get; set; }

        public bool Equals(Resistances? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Fire == other.Fire && Water == other.Water && Ice == other.Ice && Wind == other.Wind && Thunder == other.Thunder && Machine == other.Machine && Dark == other.Dark;
        }

        public override bool Equals(object? obj) => Equals(obj as Resistances);
        public override int GetHashCode() => HashCode.Combine(Fire, Water, Ice, Wind, Thunder, Machine, Dark);
    }
}
