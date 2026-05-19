namespace Tests.Integration.Infrastructure.Memory;

using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using Backend.Infrastructure.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class WindowsMemoryProviderTests
{
    [Fact]
    public void OpenExisting_ShouldReadAndWriteMemoryMappedFiles_OnWindows()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return;
        }

        string mapName = $"digivice_test_mapping_{Guid.NewGuid()}";
        long capacity = 1024;

        using MemoryMappedFile memoryMappedFile = MemoryMappedFile.CreateNew(mapName, capacity);
        using MemoryMappedViewAccessor viewAccessor = memoryMappedFile.CreateViewAccessor();

        viewAccessor.Write(0x10, 123456789);
        viewAccessor.Write(0x20, (short)9999);
        byte[] expectedBytes = [7, 8, 9, 10];
        viewAccessor.WriteArray(0x30, expectedBytes, 0, expectedBytes.Length);

        Mock<ILogger<WindowsMemoryProvider>> loggerMock = new Mock<ILogger<WindowsMemoryProvider>>();
        WindowsMemoryProvider provider = new WindowsMemoryProvider(loggerMock.Object);

        using IMemoryAccessor? accessor = provider.OpenExisting(mapName);

        Assert.NotNull(accessor);

        int readInt32 = accessor.ReadInt32(0x10);
        short readInt16 = accessor.ReadInt16(0x20);
        byte[] readBytes = new byte[expectedBytes.Length];
        accessor.ReadArray(0x30, readBytes, 0, readBytes.Length);

        Assert.Equal(123456789, readInt32);
        Assert.Equal((short)9999, readInt16);
        Assert.Equal(expectedBytes, readBytes);
    }

    [Fact]
    public void OpenExisting_ShouldReturnNull_WhenMappingDoesNotExist()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return;
        }

        string nonExistentMapName = $"non_existent_mapping_{Guid.NewGuid()}";
        Mock<ILogger<WindowsMemoryProvider>> loggerMock = new Mock<ILogger<WindowsMemoryProvider>>();
        WindowsMemoryProvider provider = new WindowsMemoryProvider(loggerMock.Object);

        IMemoryAccessor? accessor = provider.OpenExisting(nonExistentMapName);

        Assert.Null(accessor);
    }

    [Fact]
    public void OpenExisting_ShouldReturnNull_WhenMapNameIsEmpty()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return;
        }

        Mock<ILogger<WindowsMemoryProvider>> loggerMock = new Mock<ILogger<WindowsMemoryProvider>>();
        WindowsMemoryProvider provider = new WindowsMemoryProvider(loggerMock.Object);

        IMemoryAccessor? accessorNull = provider.OpenExisting(null!);
        IMemoryAccessor? accessorEmpty = provider.OpenExisting(string.Empty);
        IMemoryAccessor? accessorWhitespace = provider.OpenExisting("   ");

        Assert.Null(accessorNull);
        Assert.Null(accessorEmpty);
        Assert.Null(accessorWhitespace);
    }
}
