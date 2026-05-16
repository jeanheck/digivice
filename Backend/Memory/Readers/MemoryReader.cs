using Backend.Infrastructure.Processes;
using Backend.Infrastructure.Memory;
using Serilog;

namespace Backend.Memory.Readers
{
    public class MemoryReader(
        IProcessService processService,
        IMemoryProvider memoryProvider,
        IConfiguration configuration) : IMemoryReader
    {
        private IMemoryAccessor? accessor;
        public bool IsConnected { get; private set; }

        public bool TryConnect()
        {
            try
            {
                // 1. Search for the specific process name using the service
                var emulatorName = configuration.GetValue<string>("EmulatorProcessName");
                if (string.IsNullOrEmpty(emulatorName))
                {
                    Log.Error("EmulatorProcessName not found in appsettings.json");
                    IsConnected = false;
                    return false;
                }

                int? processId = processService.GetProcessIdByName(emulatorName);

                if (processId == null)
                {
                    IsConnected = false;
                    return false;
                }

                string dynamicMapName = $"duckstation_{processId}";

                // 2. Attempt to open the memory mapping through the provider
                accessor = memoryProvider.OpenExisting(dynamicMapName);

                if (accessor == null)
                {
                    IsConnected = false;
                    return false;
                }

                IsConnected = true;
                Log.Information("Connected to DuckStation! Mapping found: {MapName}", dynamicMapName);
                return true;
            }
            catch (Exception)
            {
                IsConnected = false;
                return false;
            }
        }

        public int? ReadInt32(long address)
        {
            if (!IsConnected || accessor == null) return null;

            try
            {
                return accessor.ReadInt32(address);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to read memory at 0x{Address:X}: {Msg}", address, ex.Message);
                return null;
            }
        }

        public void Dispose()
        {
            accessor?.Dispose();
            Log.Information("Memory resources released.");
        }

        public short? ReadInt16(long address)
        {
            if (!IsConnected || accessor == null) return null;
            try
            {
                return accessor.ReadInt16(address);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to read memory at 0x{Address:X}: {Msg}", address, ex.Message);
                return null;
            }
        }

        public byte[]? ReadBytes(long address, int length)
        {
            if (!IsConnected || accessor == null) return null;
            try
            {
                byte[] buffer = new byte[length];
                accessor.ReadArray(address, buffer, 0, length);
                return buffer;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to read bytes at 0x{Address:X}: {Msg}", address, ex.Message);
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
            catch { return 0; }
        }
    }
}
