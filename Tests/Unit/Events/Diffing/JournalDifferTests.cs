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
                Steps = [new Step { Number = 0, IsDone = false, Requisites = [] }]
            },
            SideQuests = []
        };
        var newObj = new Journal
        {
            MainQuest = new Quest
            {
                Id = "1",
                Requisites = [],
                Steps = [new Step { Number = 0, IsDone = true, Requisites = [] }]
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
        Assert.True(result.MainQuest.Value.Steps.Value[0].IsDone.HasValue);
        Assert.True(result.MainQuest.Value.Steps.Value[0].IsDone.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenSideQuestsChanged()
    {
        var previous = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [
                new Quest { Id = "2", Requisites = [], Steps = [new Step { Number = 0, IsDone = false }] }
            ]
        };
        var newObj = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [
                new Quest { Id = "2", Requisites = [], Steps = [new Step { Number = 0, IsDone = true }] }
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
        Assert.True(steps[0].IsDone.Value);
    }

    [Fact]
    public void Diff_ShouldReturnMultipleDeltas_WhenMainQuestAndSideQuestsChanged()
    {
        var previous = new Journal
        {
            MainQuest = new Quest { Id = "1", Requisites = [], Steps = [new Step { Number = 0, IsDone = false }] },
            SideQuests = [
                new Quest { Id = "2", Requisites = [], Steps = [new Step { Number = 0, IsDone = false }] }
            ]
        };
        var newObj = new Journal
        {
            MainQuest = new Quest { Id = "1", Requisites = [], Steps = [new Step { Number = 0, IsDone = true }] },
            SideQuests = [
                new Quest { Id = "2", Requisites = [], Steps = [new Step { Number = 0, IsDone = true }] }
            ]
        };

        var result = JournalDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        
        Assert.True(result.MainQuest.HasValue);
        var mainQuest = result.MainQuest.Value!;
        Assert.Equal("1", mainQuest.Id);
        Assert.True(mainQuest.Steps.Value![0].IsDone.Value);

        Assert.True(result.SideQuests.HasValue);
        var sideQuests = result.SideQuests.Value!;
        Assert.Single(sideQuests);
        Assert.Equal("2", sideQuests[0].Id);
        Assert.True(sideQuests[0].Steps.Value![0].IsDone.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenLegendaryWeaponsChanged()
    {
        var previous = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [],
            LegendaryWeapons = [
                new Quest { Id = "eternally", Requisites = [], Steps = [new Step { Number = 1, IsDone = false }] }
            ]
        };
        var newObj = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [],
            LegendaryWeapons = [
                new Quest { Id = "eternally", Requisites = [], Steps = [new Step { Number = 1, IsDone = true }] }
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
        Assert.True(legendaryWeapons[0].Steps.Value![0].IsDone.Value);
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
                    Steps = [new Step { Number = 1, IsDone = false }]
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
                    Steps = [new Step { Number = 1, IsDone = true }]
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
        Assert.True(driAgents[0].Steps.Value![0].IsDone.Value);
    }

    [Fact]
    public void Diff_ShouldReturnDelta_WhenDuelIslandChanged()
    {
        var previous = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [],
            LegendaryWeapons = [],
            DriAgents = [],
            DuelIsland = [
                new Quest
                {
                    Id = "asukaTrophy",
                    Requisites = [],
                    Steps = [new Step { Number = 1, IsDone = false }]
                }
            ]
        };
        var newObj = new Journal
        {
            MainQuest = new Quest { Id = "1" },
            SideQuests = [],
            LegendaryWeapons = [],
            DriAgents = [],
            DuelIsland = [
                new Quest
                {
                    Id = "asukaTrophy",
                    Requisites = [],
                    Steps = [new Step { Number = 1, IsDone = true }]
                }
            ]
        };

        var result = JournalDiffer.Diff(previous, newObj);

        Assert.NotNull(result);
        Assert.False(result.MainQuest.HasValue);
        Assert.False(result.SideQuests.HasValue);
        Assert.False(result.LegendaryWeapons.HasValue);
        Assert.False(result.DriAgents.HasValue);
        Assert.True(result.DuelIsland.HasValue);

        var duelIsland = result.DuelIsland.Value!;
        Assert.Single(duelIsland);
        Assert.Equal("asukaTrophy", duelIsland[0].Id);
        Assert.True(duelIsland[0].Steps.HasValue);
        Assert.True(duelIsland[0].Steps.Value![0].IsDone.Value);
    }
}
