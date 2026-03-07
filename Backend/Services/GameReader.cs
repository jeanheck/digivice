using System;
using System.Collections.Generic;
using Backend.Interfaces;
using Backend.Models.Addresses;
using Backend.Models.Resources;
using Backend.Models.Quests;

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

        private byte ReadByteSafe(string? hexAddress)
        {
            if (string.IsNullOrEmpty(hexAddress)) return 0;
            int address = Convert.ToInt32(hexAddress, 16);
            if (address == 0) return 0;
            var bytes = _memoryReader.ReadBytes(address, 1);
            return (bytes != null && bytes.Length > 0) ? bytes[0] : (byte)0;
        }

        public ImportantItemsResource ReadImportantItems(ImportantItemsAddresses addresses)
        {
            return new ImportantItemsResource
            {
                Folderbag = ReadByteSafe(addresses.Folderbag),
                TreeBoots = ReadByteSafe(addresses.TreeBoots),
                FishingPole = ReadByteSafe(addresses.FishingPole),
                RedSnapper = ReadByteSafe(addresses.RedSnapper)
            };
        }

        public ConsumableItemsResource ReadConsumableItems(ConsumableItemsAddresses addresses)
        {
            return new ConsumableItemsResource
            {
                PowerCharge = ReadByteSafe(addresses.PowerCharge),
                SpiderWeb = ReadByteSafe(addresses.SpiderWeb),
                BambooSpear = ReadByteSafe(addresses.BambooSpear)
            };
        }


        public Dictionary<int, byte> ReadQuestSteps(QuestAddresses addresses)
        {
            return ReadMainQuestSteps(addresses.Steps);
        }

        public Dictionary<int, byte> ReadMainQuestSteps(List<QuestAddressStep> steps)
        {
            var resource = new Dictionary<int, byte>();
            foreach (var step in steps)
            {
                if (string.IsNullOrEmpty(step.Address)) continue;

                int addressStr = Convert.ToInt32(step.Address, 16);
                if (addressStr == 0) continue;

                var bytes = _memoryReader.ReadBytes(addressStr, 1);
                resource[step.Id] = (bytes != null && bytes.Length > 0) ? bytes[0] : (byte)0;
            }
            return resource;
        }
    }
}
