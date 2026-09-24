namespace Tests.Events.Factory;

using Backend.Domain.Models;
using Backend.Domain.Models.Battles;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Events.DTO;
using Backend.Events.Factory;
using Backend.Events.Models;

public class DigimonBattleEventFactoryTests
{
    [Fact]
    public void Create_ShouldReturnNoEvents_WhenDigimonBattleHasNoChanges()
    {
        var previousState = new State { DigimonBattle = CreateDigimonBattle(CreateEnemy()) };
        var newState = new State { DigimonBattle = CreateDigimonBattle(CreateEnemy()) };

        var result = DigimonBattleEventFactory.Create(previousState, newState);

        Assert.Empty(result);
    }

    [Fact]
    public void Create_ShouldReturnNoEvents_WhenOutOfBattleOnBothStates()
    {
        var previousState = new State { DigimonBattle = CreateDigimonBattle(enemy: null) };
        var newState = new State { DigimonBattle = CreateDigimonBattle(enemy: null) };

        var result = DigimonBattleEventFactory.Create(previousState, newState);

        Assert.Empty(result);
    }

    [Fact]
    public void Create_ShouldReturnFullEnemy_WhenBattleStarts()
    {
        var previousState = new State { DigimonBattle = CreateDigimonBattle(enemy: null) };
        var newState = new State { DigimonBattle = CreateDigimonBattle(CreateEnemy()) };

        var result = DigimonBattleEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.DigimonBattleChanged, ev.Type);
        var dto = Assert.IsType<DigimonBattleDTO>(ev.Payload);
        Assert.True(dto.Enemy.HasValue);
        Assert.NotNull(dto.Enemy.Value);
        Assert.Equal(122, dto.Enemy.Value.Id.Value);
        Assert.Equal(201, dto.Enemy.Value.GroupId.Value);
    }

    [Fact]
    public void Create_ShouldReturnExplicitNullEnemy_WhenBattleEnds()
    {
        var previousState = new State { DigimonBattle = CreateDigimonBattle(CreateEnemy()) };
        var newState = new State { DigimonBattle = CreateDigimonBattle(enemy: null) };

        var result = DigimonBattleEventFactory.Create(previousState, newState).ToList();

        var ev = Assert.Single(result);
        Assert.Equal(EventType.DigimonBattleChanged, ev.Type);
        var dto = Assert.IsType<DigimonBattleDTO>(ev.Payload);
        Assert.True(dto.Enemy.HasValue);
        Assert.Null(dto.Enemy.Value);
        Assert.False(dto.Field.HasValue);
    }

    private static DigimonBattle CreateDigimonBattle(Enemy? enemy)
    {
        return new DigimonBattle { Field = 0x00, Enemy = enemy };
    }

    private static Enemy CreateEnemy()
    {
        return new Enemy
        {
            Id = 122,
            GroupId = 201,
            Condition = 0x01,
            Strength = 250,
            Defense = 180,
            Speed = 84,
            HP = new Vital { Current = 600, Max = 672 }
        };
    }
}
