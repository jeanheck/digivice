namespace Backend.Domain.Models.Digimons
{
    public record class BasicInfo
    {
        public int Level { get; set; }
        public int Experience { get; set; }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int CurrentHP { get; set; }
        public int CurrentMP { get; set; }
    }
}
