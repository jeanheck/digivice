using System;

namespace Backend.Models.Digimons
{
    public class BasicInfo : IEquatable<BasicInfo>
    {
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public int Experience { get; set; }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int CurrentHP { get; set; }
        public int CurrentMP { get; set; }

        public bool Equals(BasicInfo? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Experience == other.Experience && Level == other.Level && CurrentHP == other.CurrentHP && MaxHP == other.MaxHP && CurrentMP == other.CurrentMP && MaxMP == other.MaxMP;
        }

        public override bool Equals(object? obj) => Equals(obj as BasicInfo);
        public override int GetHashCode() => HashCode.Combine(Name, Experience, Level, CurrentHP, MaxHP, CurrentMP, MaxMP);
    }
}
