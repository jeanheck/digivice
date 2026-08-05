using Backend.Domain.Assemblers.Parties.Digimons;
using Backend.Domain.Models.Parties;
using Backend.Domain.Models.Parties.Digimons;
using Backend.Memory.Resources.Parties;

namespace Backend.Domain.Assemblers.Parties
{
    public static class DigimonAssembler
    {
        private const int NoActiveDigievolution = 0xFFFF;

        public static Digimon Assemble(DigimonResource resource)
        {
            var activeDigievolutionId = resource.ActiveDigievolutionId <= 0 || resource.ActiveDigievolutionId == NoActiveDigievolution
                ? 0
                : resource.ActiveDigievolutionId;

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
                InCombat = new DigimonInCombat
                {
                    Condition = resource.InCombat.Condition,
                    HP = new Vital
                    {
                        Current = resource.InCombat.HP.Current,
                        Max = resource.InCombat.HP.Max
                    },
                    MP = new Vital
                    {
                        Current = resource.InCombat.MP.Current,
                        Max = resource.InCombat.MP.Max
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
                    Head = resource.Equipments.Head,
                    Body = resource.Equipments.Body,
                    Right = resource.Equipments.Right,
                    Left = resource.Equipments.Left,
                    Accessory1 = resource.Equipments.Accessory1,
                    Accessory2 = resource.Equipments.Accessory2
                },
                Digievolutions = [.. resource.Digievolutions.Select(DigievolutionSlotAssembler.Assemble)],
                StoredDigievolutions = [.. resource.StoredDigievolutions.Select(StoredDigievolutionAssembler.Assemble)],
                ActiveDigievolutionId = activeDigievolutionId
            };
        }
    }
}
