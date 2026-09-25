using Backend.Memory.Resources;

namespace Backend.Application.Loaders.Interfaces
{
    public interface IJournalLoader
    {
        JournalResource Load();
    }
}
