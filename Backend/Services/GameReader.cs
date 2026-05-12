using Backend.Interfaces;
using Backend.Models.Addresses;
using Backend.Models.Resources;
using Backend.Models.Quests;

namespace Backend.Services
{
    public class GameReader(IMemoryReaderService memoryReader) : IGameReader
    {
        // 0x3CA is the last property (Accessory2), taking 2 bytes.
        // 1500 bytes covers the whole documented memory block safely (Including 60x20 Evolution slots).
        private const int DigimonMemoryBlockSize = 1500;

        public PlayerResource ReadPlayer(PlayerAddresses addresses)
        {
            return new PlayerResource
            {
                Bits = memoryReader.ReadInt32(addresses.Bits),
                NameInBytes = memoryReader.ReadBytes(addresses.Name, addresses.NameBufferSize),
                MapId = memoryReader.ReadInt16(addresses.MapIdAddress)
            };
        }

        public PartyResource ReadParty(PartyAddresses addresses)
        {
            var slotAddresses = new[] { addresses.PartySlot1, addresses.PartySlot2, addresses.PartySlot3 };

            return new PartyResource
            {
                DigimonIds = slotAddresses
                    .Select(slot => memoryReader.ReadBytes(slot, addresses.BytesPerSlot))
                    .Where(bytes => bytes != null && bytes.Length > 0)
                    .Select(bytes => bytes![0])
                    .ToList()
            };
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

        public DigimonResource ReadDigimon(
            int slotIndex,
            int baseAddress,
            DigievolutionsAddresses digievolutionsAddresses)
        {
            var logicBlock = memoryReader.ReadBytes(baseAddress, DigimonMemoryBlockSize);

            int activeDigievolutionId = -1;
            if (digievolutionsAddresses != null)
            {
                int ActiveDigievolutionAddress = digievolutionsAddresses.ActiveDigievolution;
                activeDigievolutionId = memoryReader.ReadInt16(baseAddress + ActiveDigievolutionAddress) ?? -1;
            }

            return new DigimonResource
            {
                SlotIndex = slotIndex,
                BaseAddress = baseAddress,
                LogicBlock = logicBlock ?? Array.Empty<byte>(),
                ActiveDigievolutionId = activeDigievolutionId
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
