namespace Tests.Events.Diffing;

using Backend.Events.Diffing;
using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Domain.Models.Journals.Quests;
using Xunit;

public class JournalDifferTests
{
    [Fact]
    public void Diff_ShouldReturnEmptyDTO_WhenNoChanges()
    {
        var previous = new Journal { MainQuest = new Quest { Id = "1" }, SideQuests = [] };
        var newObj = new Journal { MainQuest = new Quest { Id = "1" }, SideQuests = [] };

        var result = JournalDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.False(result.MainQuest.HasValue);
        Assert.False(result.SideQuests.HasValue);
    }

    [Fact]
    public void Diff_ShouldReturnFullDTO_WhenPreviousIsNull()
    {
        var newObj = new Journal { MainQuest = new Quest { Id = "1" }, SideQuests = [] };

        var result = JournalDiffer.Diff(null, newObj);

        Assert.NotNull(result);
        Assert.True(result.MainQuest.HasValue);
        Assert.NotNull(result.MainQuest.Value);
        Assert.Equal("1", result.MainQuest.Value.Id);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenMainQuestChanged()
    {
        var previous = new Journal
        {
            MainQuest = new Quest
            {
                Id = "1",
                Requisites = [],
                Steps = [new Step { Number = 0, Value = 0, Requisites = [] }]
            },
            SideQuests = []
        };
        var newObj = new Journal
        {
            MainQuest = new Quest
            {
                Id = "1",
                Requisites = [],
                Steps = [new Step { Number = 0, Value = 1, Requisites = [] }]
            },
            SideQuests = []
        };

        var result = JournalDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.True(result.MainQuest.HasValue);
        Assert.NotNull(result.MainQuest.Value);
        Assert.Equal("1", result.MainQuest.Value.Id);
        Assert.True(result.MainQuest.Value.Steps.HasValue);
        Assert.NotNull(result.MainQuest.Value.Steps.Value);
        Assert.Single(result.MainQuest.Value.Steps.Value);
        Assert.True(result.MainQuest.Value.Steps.Value[0].Value.HasValue);
        Assert.Equal((byte)1, result.MainQuest.Value.Steps.Value[0].Value.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenSideQuestsChanged()
    {
        var previous = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [
                new Quest { Id = "2", Requisites = [], Steps = [new Step { Number = 0, Value = 0 }] }
            ]
        };
        var newObj = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [
                new Quest { Id = "2", Requisites = [], Steps = [new Step { Number = 0, Value = 1 }] }
            ]
        };

        var result = JournalDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.False(result.MainQuest.HasValue);
        Assert.True(result.SideQuests.HasValue);
        
        var sideQuests = result.SideQuests.Value!;
        Assert.Single(sideQuests);
        Assert.Equal("2", sideQuests[0].Id);
        Assert.True(sideQuests[0].Steps.HasValue);
        
        var steps = sideQuests[0].Steps.Value!;
        Assert.Equal((byte)1, steps[0].Value.Value);
    }

    [Fact]
    public void Diff_ShouldReturnMultipleDeltas_WhenMainQuestAndSideQuestsChanged()
    {
        var previous = new Journal
        {
            MainQuest = new Quest { Id = "1", Requisites = [], Steps = [new Step { Number = 0, Value = 0 }] },
            SideQuests = [
                new Quest { Id = "2", Requisites = [], Steps = [new Step { Number = 0, Value = 0 }] }
            ]
        };
        var newObj = new Journal
        {
            MainQuest = new Quest { Id = "1", Requisites = [], Steps = [new Step { Number = 0, Value = 1 }] },
            SideQuests = [
                new Quest { Id = "2", Requisites = [], Steps = [new Step { Number = 0, Value = 1 }] }
            ]
        };

        var result = JournalDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        
        Assert.True(result.MainQuest.HasValue);
        var mainQuest = result.MainQuest.Value!;
        Assert.Equal("1", mainQuest.Id);
        Assert.Equal((byte)1, mainQuest.Steps.Value![0].Value.Value);

        Assert.True(result.SideQuests.HasValue);
        var sideQuests = result.SideQuests.Value!;
        Assert.Single(sideQuests);
        Assert.Equal("2", sideQuests[0].Id);
        Assert.Equal((byte)1, sideQuests[0].Steps.Value![0].Value.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenLegendaryWeaponsChanged()
    {
        var previous = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [],
            LegendaryWeapons = [
                new Quest { Id = "eternally", Requisites = [], Steps = [new Step { Number = 1, Value = 0 }] }
            ]
        };
        var newObj = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [],
            LegendaryWeapons = [
                new Quest { Id = "eternally", Requisites = [], Steps = [new Step { Number = 1, Value = 1 }] }
            ]
        };

        var result = JournalDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.False(result.MainQuest.HasValue);
        Assert.False(result.SideQuests.HasValue);
        Assert.True(result.LegendaryWeapons.HasValue);

        var legendaryWeapons = result.LegendaryWeapons.Value!;
        Assert.Single(legendaryWeapons);
        Assert.Equal("eternally", legendaryWeapons[0].Id);
        Assert.True(legendaryWeapons[0].Steps.HasValue);
        Assert.Equal((byte)1, legendaryWeapons[0].Steps.Value![0].Value.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenDriAgentsChanged()
    {
        var previous = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [],
            LegendaryWeapons = [],
            DriAgents = [
                new Quest
                {
                    Id = "driAgentGuilmon",
                    Requisites = [],
                    Steps = [new Step { Number = 1, Value = 0 }]
                }
            ]
        };
        var newObj = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [],
            LegendaryWeapons = [],
            DriAgents = [
                new Quest
                {
                    Id = "driAgentGuilmon",
                    Requisites = [],
                    Steps = [new Step { Number = 1, Value = 1 }]
                }
            ]
        };

        var result = JournalDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.False(result.MainQuest.HasValue);
        Assert.False(result.SideQuests.HasValue);
        Assert.False(result.LegendaryWeapons.HasValue);
        Assert.True(result.DriAgents.HasValue);

        var driAgents = result.DriAgents.Value!;
        Assert.Single(driAgents);
        Assert.Equal("driAgentGuilmon", driAgents[0].Id);
        Assert.True(driAgents[0].Steps.HasValue);
        Assert.Equal((byte)1, driAgents[0].Steps.Value![0].Value.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenAuctionsChanged()
    {
        var previous = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [],
            Auctions =
            [
                new Auction { Id = "divineBarrier", Value = 0x00 },
                new Auction { Id = "hazardShield", Value = 0x00 },
            ],
        };
        var newObj = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [],
            Auctions =
            [
                new Auction { Id = "divineBarrier", Value = 0x01 },
                new Auction { Id = "hazardShield", Value = 0x00 },
            ],
        };

        var result = JournalDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.False(result.MainQuest.HasValue);
        Assert.True(result.Auctions.HasValue);

        var auctions = result.Auctions.Value!;
        var delta = Assert.Single(auctions);
        Assert.Equal("divineBarrier", delta.Id);
        Assert.True(delta.Value.HasValue);
        Assert.Equal(0x01, delta.Value.Value);
    }
}
