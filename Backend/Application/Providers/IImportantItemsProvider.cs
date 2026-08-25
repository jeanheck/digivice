using Backend.Domain.Models;

namespace Backend.Application.Providers
{
    public interface IImportantItemsProvider
    {
        ImportantItems Get();
    }
}
