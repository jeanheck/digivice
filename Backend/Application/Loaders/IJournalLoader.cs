using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public interface IJournalLoader
    {
        JournalResource Load();
    }
}
