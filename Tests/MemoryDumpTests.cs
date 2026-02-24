using Backend.Services;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class MemoryDumpTests
    {
        private readonly ITestOutputHelper _output;

        public MemoryDumpTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void DumpGuilmonMemory()
        {
            var processService = new WindowsProcessProvider();
            var memoryProvider = new WindowsMemoryProvider();
            var memoryReader = new MemoryReaderService(processService, memoryProvider);

            if (!memoryReader.TryConnect())
            {
                _output.WriteLine("Could not connect to DuckStation");
                return;
            }

            int guilmonBase = 0x0004A7E8;
            int offsetStart = 0x40;
            int dumpLength = 0x300;

            byte[]? bytes = memoryReader.ReadBytes(guilmonBase + offsetStart, dumpLength);
            if (bytes == null)
            {
                _output.WriteLine("Failed to read bytes.");
                return;
            }

            var result = new System.Collections.Generic.List<string>();
            result.Add($"Dumping Memory for Guilmon starting at base + 0x{offsetStart:X}:");
            for (int i = 0; i < bytes.Length; i += 16)
            {
                string hex = "";
                for (int j = 0; j < 16 && (i + j) < bytes.Length; j++)
                {
                    hex += $"{bytes[i + j]:X2} ";
                }

                result.Add($"0x{guilmonBase + offsetStart + i:X8} [+0x{(offsetStart + i):X2}]: {hex}");
            }
            System.IO.File.WriteAllLines("dump.txt", result);
        }
    }
}
