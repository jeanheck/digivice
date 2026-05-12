using Backend.Interfaces;
using Backend.Models.Addresses;
using Backend.Models.Resources;
using Backend.Models.Quests;

namespace Backend.Services
{
    public class GameReader(IMemoryReaderService memoryReader) : IGameReader
    {
        public PlayerResource ReadPlayer(PlayerAddresses addresses)
        {
            return new PlayerResource
            {
                Bits = memoryReader.ReadInt32(addresses.Bits),
                NameBytes = memoryReader.ReadBytes(addresses.Name, addresses.NameBufferSize),
                MapId = memoryReader.ReadInt16(addresses.MapIdAddress)
            };
        }

        public PartyResource ReadParty(PartyAddresses addresses)
        {
            var resource = new PartyResource();
            var slotAddresses = new[] { addresses.PartySlot1, addresses.PartySlot2, addresses.PartySlot3 };

            foreach (var address in slotAddresses)
            {
                var idBytes = memoryReader.ReadBytes(address, addresses.PartySlotStride);

                if (idBytes != null && idBytes.Length > 0)
                {
                    resource.ActiveDigimonIds.Add(idBytes[0]);
                }
            }

            return resource;
        }


        public ImportantItemsResource ReadImportantItems(ImportantItemsAddresses addresses)
        {
            return new ImportantItemsResource
            {
                Folderbag = memoryReader.ReadByteSafe(addresses.FolderBag.Address),
                TreeBoots = memoryReader.ReadByteSafe(addresses.TreeBoots.Address),
                FishingPole = memoryReader.ReadByteSafe(addresses.FishingPole.Address),
                RedSnapper = memoryReader.ReadByteSafe(addresses.RedSnapper.Address)
            };
        }

        public ConsumableItemsResource ReadConsumableItems(ConsumableItemsAddresses addresses)
        {
            return new ConsumableItemsResource
            {
                PowerCharge = memoryReader.ReadByteSafe(addresses.PowerCharge.Address),
                SpiderWeb = memoryReader.ReadByteSafe(addresses.SpiderWeb.Address),
                BambooSpear = memoryReader.ReadByteSafe(addresses.BambooSpear.Address)
            };
        }

        public DigimonResource ReadDigimonResource(int slotIndex, int baseAddress, DigimonAddresses addresses)
        {
            // The digimon struct is quite large, 0x3CA is the last property (Accessory2), taking 2 bytes
            // Reserving 1500 bytes covers the whole documented memory block safely (Including 60x20 Evolution slots)
            var logicBlock = memoryReader.ReadBytes(baseAddress, 1500);

            int activeEvoId = -1;
            if (addresses.Digievolutions != null)
            {
                int offset = addresses.Digievolutions.ActiveDigievolution;
                activeEvoId = memoryReader.ReadInt16(baseAddress + offset) ?? -1;
            }

            return new DigimonResource
            {
                SlotIndex = slotIndex,
                BaseAddress = baseAddress,
                LogicBlock = logicBlock ?? System.Array.Empty<byte>(),
                ActiveDigievolutionId = activeEvoId
            };
        }

        public Dictionary<int, byte> ReadQuestSteps(List<QuestStep> steps)
        {
            var resource = new Dictionary<int, byte>();
            foreach (var step in steps)
            {
                if (step.Address == 0) continue;

                var bytes = memoryReader.ReadBytes(step.Address, 1);
                resource[step.Number] = (bytes != null && bytes.Length > 0) ? bytes[0] : (byte)0;
            }
            return resource;
        }
    }
}
