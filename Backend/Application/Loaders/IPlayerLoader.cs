using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public interface IPlayerLoader
    {
        PlayerResource Load();
    }
}
