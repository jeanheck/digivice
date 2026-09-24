namespace Tests.Integration.Application.Loaders;

using Backend.Memory.Repositories;
using Backend.Memory.Readers.Interfaces;
using Moq;

public abstract class LoaderIntegrationTestBase
{
    protected static AddressesRepository CreateAddressesRepository()
    {
        return new AddressesRepository(GetRealDefinitionsPath());
    }

    protected static void SetupReadByteBitMaskBridge(Mock<IMemoryReader> memoryReaderMock)
    {
        memoryReaderMock
            .Setup(memoryReader => memoryReader.ReadByte(It.IsAny<long>(), It.IsAny<long>()))
            .Returns((long address, long bitMask) =>
                (byte)(memoryReaderMock.Object.ReadByte(address) & bitMask));
    }

    protected static void WriteInt16(byte[] block, int offset, short value)
    {
        BitConverter.GetBytes(value).CopyTo(block, offset);
    }

    protected static void WriteInt32(byte[] block, int offset, int value)
    {
        BitConverter.GetBytes(value).CopyTo(block, offset);
    }

    private static string GetRealDefinitionsPath()
    {
        var currentDir = new DirectoryInfo(AppContext.BaseDirectory);
        while (currentDir != null)
        {
            var potentialPath = Path.Combine(currentDir.FullName, "Backend", "Memory", "Definitions");
            if (Directory.Exists(potentialPath))
            {
                return potentialPath;
            }

            currentDir = currentDir.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate Backend/Memory/Definitions folder in the directory tree.");
    }
}
