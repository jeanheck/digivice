using Backend.Domain.Models;

namespace Backend.Application.Providers.Interfaces
{
    public interface IImportantItemsProvider
    {
        ImportantItems Get();
    }
}
