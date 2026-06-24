namespace Tests.Domain.Assemblers;

using Backend.Domain.Assemblers;
using Backend.Memory.Resources;

public class PlayerAssemblerTests
{
    [Fact]
    public void Assemble_ShouldMapAllFieldsCorrectly_WhenResourceIsValid()
    {
        var resource = new PlayerResource
        {
            NameInBytes = [0x0E, 0x28],
            Bits = 1500,
            MapId = 255
        };

        var result = PlayerAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal("Aa", result.Name);
        Assert.Equal(1500, result.Bits);
        Assert.Equal("00FF", result.MapId);
    }

    [Fact]
    public void Assemble_ShouldFallBackToSafeDefaults_WhenFieldsAreNull()
    {
        var resource = new PlayerResource
        {
            NameInBytes = null,
            Bits = null,
            MapId = null
        };

        var result = PlayerAssembler.Assemble(resource);

        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.Name);
        Assert.Equal(0, result.Bits);
        Assert.Equal(string.Empty, result.MapId);
    }
}
