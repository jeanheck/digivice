using Backend.Domain.Models;
using Backend.Domain.Models.Digimons;
using Backend.Memory.Resources;

namespace Backend.Domain.Assemblers
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
                Vitals = new Vitals
                {
                    CurrentHP = resource.Vitals.CurrentHP,
                    MaxHP = resource.Vitals.MaxHP,
                    CurrentMP = resource.Vitals.CurrentMP,
                    MaxMP = resource.Vitals.MaxMP
                },
                Attributes = new Attributes
                {
                    Strength = resource.Attributes.Strength,
                    Defense = resource.Attributes.Defense,
                    Spirit = resource.Attributes.Spirit,
                    Wisdom = resource.Attributes.Wisdow,
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
                    RightHand = resource.Equipments.RightHand,
                    LeftHand = resource.Equipments.LeftHand,
                    Accessory1 = resource.Equipments.Accessory1,
                    Accessory2 = resource.Equipments.Accessory2
                },
                Digievolutions = resource.Digievolutions.Select(DigievolutionSlotAssembler.Assemble).ToList(),
                ActiveDigievolutionId = activeDigievolutionId
            };
        }
    }
}
