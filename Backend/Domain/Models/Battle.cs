using Backend.Domain.Models.Battles;

namespace Backend.Domain.Models
{
    public record class Battle
    {
        public byte Field { get; set; }
        public Enemy Enemy { get; set; } = new();
    }
}
