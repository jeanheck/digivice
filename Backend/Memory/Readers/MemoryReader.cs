using Backend.Infrastructure.Duckstation;
using Backend.Infrastructure.Memory;
using Serilog;

namespace Backend.Memory.Readers
{
    public class MemoryReader(IDuckstationConnector duckstationConnector) : IMemoryReader
    {
        private IMemoryAccessor? GetConnectedAccessor()
        {
            var accessor = duckstationConnector.Accessor;
            if (!duckstationConnector.IsConnected || accessor == null)
            {
                return null;
            }

            return accessor;
        }

        private void HandleReadFailure(long address, Exception ex)
        {
            Log.Error("Failed to read memory at 0x{Address:X}: {Msg}", address, ex.Message);
            duckstationConnector.InvalidateConnection();
        }

        private T? TryRead<T>(long address, Func<IMemoryAccessor, T> read) where T : struct
        {
            var accessor = GetConnectedAccessor();
            if (accessor == null)
            {
                return null;
            }

            try
            {
                return read(accessor);
            }
            catch (Exception ex)
            {
                HandleReadFailure(address, ex);
                return null;
            }
        }

        public int? ReadInt32(long address) =>
            TryRead(address, accessor => accessor.ReadInt32(address));

        public short? ReadInt16(long address) =>
            TryRead(address, accessor => accessor.ReadInt16(address));

        public byte[]? ReadBytes(long address, int length)
        {
            var accessor = GetConnectedAccessor();
            if (accessor == null)
            {
                return null;
            }

            try
            {
                byte[] buffer = new byte[length];
                accessor.ReadArray(address, buffer, 0, length);
                return buffer;
            }
            catch (Exception ex)
            {
                HandleReadFailure(address, ex);
                return null;
            }
        }

        public byte ReadByteSafe(long address, long? bitMask = null)
        {
            if (address == 0)
            {
                return 0;
            }

            try
            {
                var bytes = ReadBytes(address, 1);
                if (bytes == null || bytes.Length == 0)
                {
                    return 0;
                }

                byte rawValue = bytes[0];
                if (bitMask == null)
                {
                    return rawValue;
                }

                return (byte)(rawValue & bitMask.Value);
            }
            catch
            {
                return 0;
            }
        }
    }
}
