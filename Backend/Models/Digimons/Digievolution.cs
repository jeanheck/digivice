namespace Backend.Models.Digimons
{
    public class Digievolution
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public List<Technique> Techniques { get; set; } = new();
    }
}

