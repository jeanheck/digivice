using Backend.Infrastructure.Duckstation;
using Serilog;

namespace Backend.Memory.Readers
{
    public class MemoryReader(IDuckstationConnector duckstationConnector) : IMemoryReader
    {
        public int? ReadInt32(long address)
        {
            var accessor = duckstationConnector.Accessor;
            if (!duckstationConnector.IsConnected || accessor == null)
            {
                return null;
            }

            try
            {
                return accessor.ReadInt32(address);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to read memory at 0x{Address:X}: {Msg}", address, ex.Message);
                duckstationConnector.InvalidateConnection();
                return null;
            }
        }

        public short? ReadInt16(long address)
        {
            var accessor = duckstationConnector.Accessor;
            if (!duckstationConnector.IsConnected || accessor == null)
            {
                return null;
            }

            try
            {
                return accessor.ReadInt16(address);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to read memory at 0x{Address:X}: {Msg}", address, ex.Message);
                duckstationConnector.InvalidateConnection();
                return null;
            }
        }

        public byte[]? ReadBytes(long address, int length)
        {
            var accessor = duckstationConnector.Accessor;
            if (!duckstationConnector.IsConnected || accessor == null)
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
                Log.Error("Failed to read bytes at 0x{Address:X}: {Msg}", address, ex.Message);
                duckstationConnector.InvalidateConnection();
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
