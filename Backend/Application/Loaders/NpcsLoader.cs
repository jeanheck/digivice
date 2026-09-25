using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;
using Backend.Memory.Readers.Interfaces;

namespace Backend.Application.Loaders
{
    public class NpcsLoader(
        IAddressesRepository addressesRepository,
        INpcsReader npcsReader) : INpcsLoader
    {
        public NpcsResource Load()
        {
            return npcsReader.Read(addressesRepository.GetNpcsAddresses());
        }
    }
}
