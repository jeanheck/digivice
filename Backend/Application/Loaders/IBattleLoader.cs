using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public interface IBattleLoader
    {
        BattleResource Load();
    }
}
