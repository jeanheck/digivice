using Backend.Memory.Readers;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class ImportantItemsLoader(
        IAddressesRepository addressesRepository,
        IImportantItemsReader importantItemsReader) : IImportantItemsLoader
    {
        public ImportantItemsResource Load()
        {
            return importantItemsReader.Read(addressesRepository.GetImportantItemsAddresses());
        }
    }
}
