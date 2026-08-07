using Backend.Domain.Models;
using Backend.Domain.Models.Battles;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
{
    public static class BattleAssembler
    {
        public static Battle Assemble(BattleResource resource)
        {
            return new Battle
            {
                Enemy = new Enemy
                {
                    Id = resource.Enemy.Id,
                    Condition = resource.Enemy.Condition,
                    Strength = resource.Enemy.Strength,
                    Defense = resource.Enemy.Defense,
                    Speed = resource.Enemy.Speed,
                    HP = new Vital
                    {
                        Current = resource.Enemy.HP.Current,
                        Max = resource.Enemy.HP.Max
                    },
                    MP = new Vital
                    {
                        Current = resource.Enemy.MP.Current,
                        Max = resource.Enemy.MP.Max
                    }
                }
            };
        }
    }
}
