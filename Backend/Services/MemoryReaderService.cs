using Backend.Interfaces;
using Serilog;

namespace Backend.Services
{
    public class MemoryReaderService : IMemoryReaderService
    {
        private readonly IProcessService _processService;
        private readonly IMemoryProvider _memoryProvider;
        private IMemoryAccessor? _accessor;
        public bool IsConnected { get; private set; }

        public MemoryReaderService(IProcessService processService, IMemoryProvider memoryProvider)
        {
            _processService = processService;
            _memoryProvider = memoryProvider;
        }

        public bool TryConnect()
        {
            try
            {
                // 1. Search for the specific process name using the service
                int? processId = _processService.GetProcessIdByName("duckstation-qt-x64-ReleaseLTCG");

                if (processId == null)
                {
                    IsConnected = false;
                    return false;
                }

                string dynamicMapName = $"duckstation_{processId}";

                // 2. Attempt to open the memory mapping through the provider
                _accessor = _memoryProvider.OpenExisting(dynamicMapName);

                if (_accessor == null)
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

        public int ReadInt32(int address)
        {
            if (!IsConnected || _accessor == null) return -1;

            try
            {
                return _accessor.ReadInt32(address);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to read memory at 0x{Address:X}: {Msg}", address, ex.Message);
                return -1;
            }
        }

        public void Dispose()
        {
            _accessor?.Dispose();
            Log.Information("Memory resources released.");
        }

        public short ReadInt16(int address)
        {
            if (!IsConnected || _accessor == null) return -1;
            try
            {
                return _accessor.ReadInt16(address);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public byte[]? ReadBytes(int address, int length)
        {
            if (!IsConnected || _accessor == null) return null;
            try
            {
                byte[] buffer = new byte[length];
                _accessor.ReadArray(address, buffer, 0, length);
                return buffer;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to read bytes at 0x{Address:X}: {Msg}", address, ex.Message);
                return null;
            }
        }

        public string ReadString(int address, int length, System.Text.Encoding? encoding = null)
        {
            var bytes = ReadBytes(address, length);
            if (bytes == null) return string.Empty;

            encoding ??= System.Text.Encoding.ASCII;

            // Encontra o null terminator (0x00)
            int nullIdx = Array.IndexOf(bytes, (byte)0);
            int len = nullIdx >= 0 ? nullIdx : bytes.Length;

            return encoding.GetString(bytes, 0, len);
        }
    }
}
