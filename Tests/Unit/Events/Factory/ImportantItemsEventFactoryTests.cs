namespace Tests.Events.Factory;

using Backend.Domain.Models;
using Backend.Events.DTO;
using Backend.Events.Factory;
using Backend.Events.Models;

public class ImportantItemsEventFactoryTests
{
    [Fact]
    public void Create_ShouldReturnNoEvents_WhenImportantItemsHaveNoChanges()
    {
        var previousState = CreateState(new ImportantItems { TreeBoots = true });
        var newState = CreateState(new ImportantItems { TreeBoots = true });

        var result = ImportantItemsEventFactory.Create(previousState, newState);

        Assert.Empty(result);
    }

    [Fact]
    public void Create_ShouldReturnImportantItemsChangedEvent_WhenImportantItemsChange()
    {
        var previousState = CreateState(new ImportantItems { TreeBoots = false });
        var newState = CreateState(new ImportantItems { TreeBoots = true });

        var result = ImportantItemsEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.ImportantItemsChanged, ev.Type);

        var dto = Assert.IsType<ImportantItemsDTO>(ev.Payload);
        Assert.True(dto.TreeBoots.HasValue);
        Assert.True(dto.TreeBoots.Value);
    }

    private static State CreateState(ImportantItems importantItems)
    {
        return new State { ImportantItems = importantItems };
    }
}
