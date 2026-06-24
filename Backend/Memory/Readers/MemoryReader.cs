using Backend.Infrastructure.Duckstation;
using Backend.Infrastructure.Memory;
using Serilog;

namespace Backend.Memory.Readers
{
    public class MemoryReader(IDuckstationSession duckstationSession) : IMemoryReader
    {
        private IMemoryAccessor GetConnectedAccessor(long address)
        {
            var accessor = duckstationSession.Accessor;
            if (accessor == null)
            {
                throw new MemoryReadException(address, "Memory session is not connected.");
            }

            return accessor;
        }

        private T ReadValue<T>(long address, Func<IMemoryAccessor, T> read) where T : struct
        {
            var accessor = GetConnectedAccessor(address);

            try
            {
                return read(accessor);
            }
            catch (MemoryReadException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to read memory at 0x{Address:X}: {Msg}", address, ex.Message);
                throw new MemoryReadException(address, $"Failed to read memory at 0x{address:X}.", ex);
            }
        }

        public int ReadInt32(long address) =>
            ReadValue(address, accessor => accessor.ReadInt32(address));

        public short ReadInt16(long address) =>
            ReadValue(address, accessor => accessor.ReadInt16(address));

        public byte[] ReadBytes(long address, int length)
        {
            var accessor = GetConnectedAccessor(address);

            try
            {
                byte[] buffer = new byte[length];
                accessor.ReadArray(address, buffer, 0, length);
                return buffer;
            }
            catch (MemoryReadException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to read memory at 0x{Address:X}: {Msg}", address, ex.Message);
                throw new MemoryReadException(address, $"Failed to read memory at 0x{address:X}.", ex);
            }
        }
    }
}
