using Backend.Application.Loaders.Interfaces;
using Backend.Domain.Assemblers;
using Backend.Domain.Models;

namespace Backend.Application.Providers
{
    public class ImportantItemsProvider(IImportantItemsLoader importantItemsLoader) : IImportantItemsProvider
    {
        public ImportantItems Get()
        {
            return ImportantItemsAssembler.Assemble(importantItemsLoader.Load());
        }
    }
}
