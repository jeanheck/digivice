using System;
using System.Collections.Generic;
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

                int mapIdAddress = Convert.ToInt32(addresses.MapIdAddress, 16);
                resource.MapId = _memoryReader.ReadInt16(mapIdAddress);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Failed to read Player memory addresses.");
            }

            return resource;
        }

        public PartyResource ReadParty(PartyAddresses addresses)
        {
            var resource = new PartyResource();

            try
            {
                int slot1 = Convert.ToInt32(addresses.PartySlot1, 16);
                int slot2 = Convert.ToInt32(addresses.PartySlot2, 16);
                int slot3 = Convert.ToInt32(addresses.PartySlot3, 16);

                var slotAddresses = new[] { slot1, slot2, slot3 };

                foreach (var address in slotAddresses)
                {
                    var idBytes = _memoryReader.ReadBytes(address, addresses.PartySlotStride);

                    if (idBytes != null && idBytes.Length > 0)
                    {
                        resource.ActiveDigimonIds.Add(idBytes[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Failed to read Party memory addresses.");
            }

            return resource;
        }

        public Dictionary<string, byte> ReadImportantItems(Dictionary<string, string> addresses)
        {
            var resource = new Dictionary<string, byte>();
            foreach (var kvp in addresses)
            {
                int addressStr = Convert.ToInt32(kvp.Value, 16);
                if (addressStr == 0) continue;

                var bytes = _memoryReader.ReadBytes(addressStr, 1);
                resource[kvp.Key] = (bytes != null && bytes.Length > 0) ? bytes[0] : (byte)0;
            }
            return resource;
        }

        public Dictionary<string, byte> ReadConsumableItems(Dictionary<string, string> addresses)
        {
            var resource = new Dictionary<string, byte>();
            foreach (var kvp in addresses)
            {
                int addressStr = Convert.ToInt32(kvp.Value, 16);
                if (addressStr == 0) continue;

                var bytes = _memoryReader.ReadBytes(addressStr, 1);
                resource[kvp.Key] = (bytes != null && bytes.Length > 0) ? bytes[0] : (byte)0;
            }
            return resource;
        }
    }
}
