using Backend.Memory.Readers;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;

namespace Backend.Application.Loaders
{
    public class NpcLoader(IAddressesRepository addressesRepository, INpcReader npcReader) : INpcLoader
    {
        public List<NpcResource> LoadNpcs()
        {
            return [.. addressesRepository.GetNpcAddresses().Select(npcReader.Read)];
        }
    }
}
