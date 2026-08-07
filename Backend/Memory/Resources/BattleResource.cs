using Backend.Memory.Resources.Battles;

namespace Backend.Memory.Resources
{
    public class BattleResource
    {
        public EnemyResource Enemy { get; set; } = new();
    }
}
