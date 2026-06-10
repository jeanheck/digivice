using System.IO.MemoryMappedFiles;

namespace Backend.Infrastructure.Memory;

public class WindowsMemoryProvider(ILogger<WindowsMemoryProvider> logger) : IMemoryProvider
{
    public IMemoryAccessor? OpenExisting(string mapName)
    {
        if (string.IsNullOrWhiteSpace(mapName))
        {
            return null;
        }

        try
        {
#pragma warning disable CA1416 // OpenExisting is only supported on Windows.
            var memoryMappedFile = MemoryMappedFile.OpenExisting(mapName);
#pragma warning restore CA1416
            var memoryMappedViewAccessor = memoryMappedFile.CreateViewAccessor();

            return new WindowsMemoryAccessor(memoryMappedFile, memoryMappedViewAccessor);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
        catch (UnauthorizedAccessException exception)
        {
            logger.LogError(exception, "Access denied when opening memory mapped file: {MapName}", mapName);
            return null;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unexpected error opening memory mapped file: {MapName}", mapName);
            return null;
        }
    }
}


