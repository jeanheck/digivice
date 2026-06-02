namespace Tests.Events.Converters.Parties.Digimons;

using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.Converters.Parties.Digimons;

public class StoredDigievolutionConverterTests
{
    [Fact]
    public void ToDTO_ShouldMapDigievolutionIdAndLevel()
    {
        var dto = StoredDigievolutionConverter.ToDTO(new StoredDigievolution
        {
            DigievolutionId = 386,
            Level = 14
        });

        Assert.Equal(386, dto.DigievolutionId.Value);
        Assert.Equal(14, dto.Level.Value);
    }
}
