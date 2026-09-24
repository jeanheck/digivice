namespace Tests.Domain.Models;

using System.Reflection;
using Backend.Domain.Models;
using Backend.Domain.Models.Journals;
using Backend.Domain.Models.Journals.Quests;
using Backend.Domain.Models.Parties;
using Backend.Domain.Models.Parties.Digimons;

// Models with hand-written Equals: a property missing from Equals makes HasNoChanges
// hide real changes, so every public property must have a mutator here.
public class ModelEqualityTests
{
    private static void AssertEveryPropertyAffectsEquality<T>(Func<T> create, Dictionary<string, Action<T>> mutators)
    {
        var propertyNames = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(property => property.Name)
            .Order();

        Assert.Equal(propertyNames, mutators.Keys.Order());
        Assert.Equal(create(), create());

        foreach (var (propertyName, mutate) in mutators)
        {
            var changed = create();
            mutate(changed);

            Assert.False(create()!.Equals(changed), $"{typeof(T).Name}.{propertyName} is not compared in Equals.");
        }
    }

    private static Digimon CreateDigimon()
    {
        return new Digimon
        {
            Level = 10,
            TP = 5,
            Blast = 100,
            Experience = 1000,
            HP = new Vital { Current = 100, Max = 120 },
            MP = new Vital { Current = 50, Max = 60 },
            InBattle = new InBattle { Condition = 0, HP = new Vital(), MP = new Vital() },
            Attributes = new Attributes { Strength = 5 },
            Resistances = new Resistances { Fire = 1 },
            Equipments = new Equipments { Head = 101 },
            Digievolutions = [new DigievolutionSlot { Index = 1, DigievolutionId = 4, Digievolution = new Digievolution { Level = 5 } }],
            StoredDigievolutions = [new StoredDigievolution { DigievolutionId = 4, Level = 5 }],
            ActiveDigievolutionId = 4
        };
    }

    private static Quest CreateQuest()
    {
        return new Quest
        {
            Id = "treeBoots",
            Requisites = [new Requisite { Id = "folderBag", IsDone = false }],
            Steps = [new Step { Number = 1, IsDone = false, Requisites = [] }]
        };
    }

    [Fact]
    public void Digimon_ShouldCompareEveryProperty()
    {
        AssertEveryPropertyAffectsEquality(CreateDigimon, new Dictionary<string, Action<Digimon>>
        {
            [nameof(Digimon.Level)] = digimon => digimon.Level++,
            [nameof(Digimon.TP)] = digimon => digimon.TP++,
            [nameof(Digimon.Blast)] = digimon => digimon.Blast++,
            [nameof(Digimon.Experience)] = digimon => digimon.Experience++,
            [nameof(Digimon.HP)] = digimon => digimon.HP.Current++,
            [nameof(Digimon.MP)] = digimon => digimon.MP.Current++,
            [nameof(Digimon.InBattle)] = digimon => digimon.InBattle.Condition = 0x04,
            [nameof(Digimon.Attributes)] = digimon => digimon.Attributes.Strength++,
            [nameof(Digimon.Resistances)] = digimon => digimon.Resistances.Fire++,
            [nameof(Digimon.Equipments)] = digimon => digimon.Equipments.Head = null,
            [nameof(Digimon.Digievolutions)] = digimon => digimon.Digievolutions[0].Digievolution!.Dvxp++,
            [nameof(Digimon.StoredDigievolutions)] = digimon => digimon.StoredDigievolutions[0].Level++,
            [nameof(Digimon.ActiveDigievolutionId)] = digimon => digimon.ActiveDigievolutionId = null,
        });
    }

    [Fact]
    public void Party_ShouldCompareEveryProperty()
    {
        AssertEveryPropertyAffectsEquality(
            () => new Party { Slots = [new DigimonSlot { Index = 1, DigimonId = 1, Digimon = CreateDigimon() }] },
            new Dictionary<string, Action<Party>>
            {
                [nameof(Party.Slots)] = party => party.Slots[0].Digimon!.Level++,
            });
    }

    [Fact]
    public void Digievolution_ShouldCompareEveryProperty()
    {
        AssertEveryPropertyAffectsEquality(
            () => new Digievolution { Level = 5, Dvxp = 100 },
            new Dictionary<string, Action<Digievolution>>
            {
                [nameof(Digievolution.Level)] = digievolution => digievolution.Level++,
                [nameof(Digievolution.Dvxp)] = digievolution => digievolution.Dvxp++,
            });
    }

    [Fact]
    public void StoredDigievolution_ShouldCompareEveryProperty()
    {
        AssertEveryPropertyAffectsEquality(
            () => new StoredDigievolution { DigievolutionId = 4, Level = 5 },
            new Dictionary<string, Action<StoredDigievolution>>
            {
                [nameof(StoredDigievolution.DigievolutionId)] = stored => stored.DigievolutionId++,
                [nameof(StoredDigievolution.Level)] = stored => stored.Level++,
            });
    }

    [Fact]
    public void Step_ShouldCompareEveryProperty()
    {
        AssertEveryPropertyAffectsEquality(
            () => new Step { Number = 1, IsDone = false, Requisites = [new Requisite { Id = "bambooSpear", IsDone = false }] },
            new Dictionary<string, Action<Step>>
            {
                [nameof(Step.Number)] = step => step.Number++,
                [nameof(Step.IsDone)] = step => step.IsDone = true,
                [nameof(Step.Requisites)] = step => step.Requisites[0].IsDone = true,
            });
    }

    [Fact]
    public void Quest_ShouldCompareEveryProperty()
    {
        AssertEveryPropertyAffectsEquality(CreateQuest, new Dictionary<string, Action<Quest>>
        {
            [nameof(Quest.Id)] = quest => quest.Id = "fishingPole",
            [nameof(Quest.Requisites)] = quest => quest.Requisites[0].IsDone = true,
            [nameof(Quest.Steps)] = quest => quest.Steps[0].IsDone = true,
        });
    }

    [Fact]
    public void Journal_ShouldCompareEveryProperty()
    {
        AssertEveryPropertyAffectsEquality(
            () => new Journal
            {
                MainQuest = CreateQuest(),
                SideQuests = [CreateQuest()],
                LegendaryWeapons = [CreateQuest()],
                DriAgents = [CreateQuest()],
                DuelIsland = [CreateQuest()]
            },
            new Dictionary<string, Action<Journal>>
            {
                [nameof(Journal.MainQuest)] = journal => journal.MainQuest.Steps[0].IsDone = true,
                [nameof(Journal.SideQuests)] = journal => journal.SideQuests[0].Steps[0].IsDone = true,
                [nameof(Journal.LegendaryWeapons)] = journal => journal.LegendaryWeapons[0].Steps[0].IsDone = true,
                [nameof(Journal.DriAgents)] = journal => journal.DriAgents[0].Steps[0].IsDone = true,
                [nameof(Journal.DuelIsland)] = journal => journal.DuelIsland[0].Steps[0].IsDone = true,
            });
    }

    [Fact]
    public void Npc_ShouldCompareEveryProperty()
    {
        AssertEveryPropertyAffectsEquality(
            () => new Npc { Battles = [new NpcBattle { Id = "first", Won = false }] },
            new Dictionary<string, Action<Npc>>
            {
                [nameof(Npc.Battles)] = npc => npc.Battles[0].Won = true,
            });
    }
}
