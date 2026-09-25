using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Interfaces;

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
