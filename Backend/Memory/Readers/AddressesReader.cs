using Backend.Memory.Addresses;
using Backend.Memory.Addresses.Digimon;
using Backend.Memory.Resources;

namespace Backend.Memory.Readers
{
    public class AddressesReader(IMemoryReader memoryReader) : IAddressesReader
    {
        private const int DigimonMemoryBlockSize = 1500;

        public PartyResource ReadPartyResource(PartyAddresses addresses)
        {
            var slotAddresses = new[] { addresses.PartySlot1, addresses.PartySlot2, addresses.PartySlot3 };

            return new PartyResource
            {
                DigimonIds = [.. slotAddresses
                    .Select(slot => memoryReader.ReadBytes(slot, addresses.BytesPerSlot))
                    .Where(bytes => bytes != null && bytes.Length > 0)
                    .Select(bytes => bytes![0])]
            };
        }

        public DigimonResource ReadDigimonResource(
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
    }
}
