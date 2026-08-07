using Backend.Domain.Models.Battles;

namespace Backend.Domain.Models
{
    public record class Battle
    {
        public Enemy Enemy { get; set; } = new();
    }
}
