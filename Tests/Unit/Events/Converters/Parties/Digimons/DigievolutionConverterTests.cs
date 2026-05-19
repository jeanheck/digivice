namespace Tests.Events.Converters.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;

public class DigievolutionConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapLevel()
    {
        var dto = DigievolutionConverter.ToDTO(new Digievolution { Level = 12 });

        Assert.Equal(12, dto.Level.Value);
    }
}
