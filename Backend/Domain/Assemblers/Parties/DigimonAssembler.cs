using Backend.Domain.Assemblers.Parties.Digimons;
using Backend.Domain.Models.Parties;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Memory.Resources.Parties;

namespace Backend.Domain.Assemblers.Parties
{
    public static class DigimonAssembler
    {
        public static Digimon Assemble(DigimonResource resource)
        {
            var activeDigievolutionId = resource.ActiveDigievolutionId <= 0
                ? null
                : (int?)resource.ActiveDigievolutionId;

            return new Digimon
            {
                Experience = resource.Experience,
                Level = resource.Level,
                TP = resource.TP,
                Blast = resource.Blast,
                HP = new Vital
                {
                    Current = resource.HP.Current,
                    Max = resource.HP.Max
                },
                MP = new Vital
                {
                    Current = resource.MP.Current,
                    Max = resource.MP.Max
                },
                InBattle = new InBattle
                {
                    Condition = resource.InBattle.Condition,
                    Strength = resource.InBattle.Strength,
                    Defense = resource.InBattle.Defense,
                    Speed = resource.InBattle.Speed,
                    HP = new Vital
                    {
                        Current = resource.InBattle.HP.Current,
                        Max = resource.InBattle.HP.Max
                    },
                    MP = new Vital
                    {
                        Current = resource.InBattle.MP.Current,
                        Max = resource.InBattle.MP.Max
                    }
                },
                Attributes = new Attributes
                {
                    Strength = resource.Attributes.Strength,
                    Defense = resource.Attributes.Defense,
                    Spirit = resource.Attributes.Spirit,
                    Wisdom = resource.Attributes.Wisdom,
                    Speed = resource.Attributes.Speed,
                    Charisma = resource.Attributes.Charisma
                },
                Resistances = new Resistances
                {
                    Fire = resource.Resistances.Fire,
                    Water = resource.Resistances.Water,
                    Ice = resource.Resistances.Ice,
                    Wind = resource.Resistances.Wind,
                    Thunder = resource.Resistances.Thunder,
                    Machine = resource.Resistances.Machine,
                    Dark = resource.Resistances.Dark
                },
                Equipments = new Equipments
                {
                    Head = resource.Equipments.Head <= 0 ? null : resource.Equipments.Head,
                    Body = resource.Equipments.Body <= 0 ? null : resource.Equipments.Body,
                    Right = resource.Equipments.Right <= 0 ? null : resource.Equipments.Right,
                    Left = resource.Equipments.Left <= 0 ? null : resource.Equipments.Left,
                    Accessory1 = resource.Equipments.Accessory1 <= 0 ? null : resource.Equipments.Accessory1,
                    Accessory2 = resource.Equipments.Accessory2 <= 0 ? null : resource.Equipments.Accessory2
                },
                Digievolutions = [.. resource.Digievolutions.Select(slot =>
                    DigievolutionSlotAssembler.Assemble(slot, resource.StoredDigievolutions))],
                StoredDigievolutions = [.. resource.StoredDigievolutions.Select(StoredDigievolutionAssembler.Assemble)],
                ActiveDigievolutionId = activeDigievolutionId
            };
        }
    }
}
