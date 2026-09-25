namespace Tests.Events.Converters.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;

public class DigievolutionConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapLevel()
    {
        var dto = DigievolutionConverter.ToDTO(new Digievolution { Level = 12, Dvexp = 139 });

        Assert.Equal(12, dto.Level.Value);
        Assert.Equal(139, dto.Dvexp.Value);
    }
}
