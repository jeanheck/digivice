using Backend.Interfaces;
using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Quests;
using Backend.Memory.Addresses.Digimon;
using Backend.Memory.Resources;
using Backend.Memory.Resources.Quests;
using Backend.Domain.Models.Quests;

namespace Backend.Memory.Readers
{
    public class ResourceReader(IMemoryReader memoryReader) : IResourceReader
    {
        private const int DigimonMemoryBlockSize = 1500;

        public PlayerResource ReadPlayer(PlayerAddresses addresses)
        {
            return new PlayerResource
            {
                Bits = memoryReader.ReadInt32(addresses.Bits),
                NameInBytes = memoryReader.ReadBytes(addresses.Name, addresses.NameBufferSize),
                MapId = memoryReader.ReadInt16(addresses.MapId)
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

        public DigimonResource ReadDigimon(
            int slotIndex,
            int baseAddress,
            DigievolutionsAddresses digievolutionsAddresses)
        {
            var logicBlock = memoryReader.ReadBytes(baseAddress, DigimonMemoryBlockSize);
            if (logicBlock == null || logicBlock.Length == 0)
            {
                return new DigimonResource();
            }

            var activeDigievolutionId = memoryReader.ReadInt16(baseAddress + digievolutionsAddresses.ActiveDigievolution) ?? 0;

            return new DigimonResource
            {
                BaseAddress = baseAddress,
                LogicBlock = logicBlock,
                ActiveDigievolutionId = activeDigievolutionId
            };
        }

        public QuestResource ReadQuest(QuestAddresses addresses)
        {
            return new QuestResource
            {
                Id = addresses.Id,
                Requisites = ReadRequisiteResources(addresses.Requisites),
                Steps = addresses.Steps.Select(step => new StepResource
                {
                    Number = step.Number,
                    Value = memoryReader.ReadByteSafe(step.Address, step.BitMask),
                    Requisites = ReadRequisiteResources(step.Requisites)
                }).ToList()
            };
        }

        private List<RequisiteResource> ReadRequisiteResources(IEnumerable<RequisiteAddresses>? requisites)
        {
            if (requisites == null) return [];

            return requisites.Select(req => new RequisiteResource
            {
                Id = req.Id,
                Value = memoryReader.ReadByteSafe(req.Address)
            }).ToList();
        }
    }
}
