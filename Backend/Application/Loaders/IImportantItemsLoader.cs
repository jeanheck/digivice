using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public interface IImportantItemsLoader
    {
        ImportantItemsResource Load();
    }
}
