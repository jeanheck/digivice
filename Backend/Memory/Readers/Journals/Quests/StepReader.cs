using Backend.Memory.Addresses.Journals.Quests;
using Backend.Memory.Readers.Helpers;
using Backend.Memory.Resources.Journals.Quests;

namespace Backend.Memory.Readers.Journals.Quests
{
    public class StepReader(
        IMemoryReader memoryReader, 
        IRequisiteReader requisiteReader) : IStepReader
    {
        public StepResource Read(StepAddresses addresses)
        {
            return new StepResource
            {
                Number = addresses.Number,
                Value = ReadValue(addresses.Address, addresses.BitMasks),
                Requisites = [.. addresses.Requisites.Select(requisiteReader.Read)]
            };
        }

        private byte ReadValue(long address, List<long> bitMasks)
        {
            if (bitMasks.Count == 0)
            {
                return FlagByteHelper.Read(memoryReader, address);
            }

            if (bitMasks.Count == 1)
            {
                return FlagByteHelper.Read(memoryReader, address, bitMasks[0]);
            }

            byte rawValue = FlagByteHelper.Read(memoryReader, address);
            foreach (long bitMask in bitMasks)
            {
                if ((rawValue & bitMask) == 0)
                {
                    return 0;
                }
            }

            return (byte)(rawValue & bitMasks[0]);
        }
    }
}
