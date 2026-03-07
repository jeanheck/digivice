using System;
using Backend.Interfaces;
using Backend.Models.Addresses;
using Backend.Models.Resources;

namespace Backend.Services
{
    public class GameReader
    {
        private readonly IMemoryReaderService _memoryReader;

        public GameReader(IMemoryReaderService memoryReader)
        {
            _memoryReader = memoryReader;
        }

        public PlayerResource ReadPlayer(PlayerAddresses addresses)
        {
            var resource = new PlayerResource();

            try
            {
                int bitsAddress = Convert.ToInt32(addresses.Bits, 16);
                resource.Bits = _memoryReader.ReadInt32(bitsAddress);

                int nameAddress = Convert.ToInt32(addresses.Name, 16);
                resource.NameBytes = _memoryReader.ReadBytes(nameAddress, addresses.NameBufferSize);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Failed to read Player memory addresses.");
            }

            return resource;
        }
    }
}
