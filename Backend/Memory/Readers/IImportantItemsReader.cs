using Backend.Memory.Addresses;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public interface IImportantItemsReader
    {
        ImportantItemsResource Read(ImportantItemsAddresses addresses);
    }
}
