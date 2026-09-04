using Backend.Application.Loaders.Interfaces;
using Backend.Memory.Readers;
using Backend.Memory.Repositories;
using Backend.Memory.Resources;

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
