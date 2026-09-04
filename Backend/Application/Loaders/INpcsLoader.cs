using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public interface INpcsLoader
    {
        NpcsResource Load();
    }
}
