using Backend.Interfaces;
using System.IO.MemoryMappedFiles;

namespace Backend.Infrastructure.Memory
{
    public class WindowsMemoryProvider : IMemoryProvider
    {
        public IMemoryAccessor? OpenExisting(string mapName)
        {
            try
            {
                var mmf = MemoryMappedFile.OpenExisting(mapName);
                var accessor = mmf.CreateViewAccessor();
                return new WindowsMemoryAccessor(accessor);
            }
            catch
            {
                return null;
            }
        }
    }
}
