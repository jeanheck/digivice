using Backend.Memory.Resources.Battles;

namespace Backend.Memory.Resources
{
    public class DigimonBattleResource
    {
        public byte Field { get; set; }
        public EnemyResource Enemy { get; set; } = new();
    }
}
