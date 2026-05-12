using Backend.Models;
using Backend.Models.Digimons;
using Backend.Models.Addresses;
using Backend.Utils;
using Backend.Interfaces;

namespace Backend.Services
{
    public class DigimonStateService
    {
        private readonly IGameDatabase _database;
        private readonly IGameReader _reader;
        // GameReader fetches generic Resources, but parsing the Digimon properties
        // demands a tight mapping. We use DigimonResource which has a memory logic block.
        // It's effectively parsed here in the Service layer to create the final Digimon Model.

        private readonly DigievolutionStateService _digievolutionStateService;

        public DigimonStateService(IGameDatabase database, IGameReader reader, DigievolutionStateService digievolutionStateService)
        {
            _database = database;
            _reader = reader;
            _digievolutionStateService = digievolutionStateService;
        }

        public Digimon GetDigimon(int slotIndex, byte digimonId, int baseAddress)
        {
            var addresses = _database.GetDigimonAddresses();
            var resource = _reader.ReadDigimonResource(slotIndex, baseAddress, addresses);

            var digimonName = GetDigimonNameById(addresses, digimonId);

            var equippedDigievolutions = _digievolutionStateService.GetEquippedDigievolutions(resource.LogicBlock, addresses.Digievolutions);

            return new Digimon
            {
                SlotIndex = slotIndex,
                ActiveDigievolutionId = resource.ActiveDigievolutionId <= 0 || resource.ActiveDigievolutionId == 65535 ? null : resource.ActiveDigievolutionId,
                BasicInfo = new BasicInfo
                {
                    Name = digimonName,
                    Experience = MemoryUtils.ReadInt32FromBlock(resource.LogicBlock, addresses.BasicInfo?.Experience ?? 0),
                    Level = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.Level ?? 0),
                    CurrentHP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.CurrentHP ?? 0),
                    MaxHP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.MaxHP ?? 0),
                    CurrentMP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.CurrentMP ?? 0),
                    MaxMP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.MaxMP ?? 0)
                },
                Attributes = new Attributes
                {
                    Strength = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Strength ?? 0),
                    Defense = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Defense ?? 0),
                    Spirit = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Spirit ?? 0),
                    Wisdom = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Wisdow ?? 0),
                    Speed = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Speed ?? 0),
                    Charisma = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Charisma ?? 0)
                },
                Resistances = new Resistances
                {
                    Fire = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Fire ?? 0),
                    Water = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Water ?? 0),
                    Ice = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Ice ?? 0),
                    Wind = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Wind ?? 0),
                    Thunder = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Thunder ?? 0),
                    Machine = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Machine ?? 0),
                    Dark = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Dark ?? 0)
                },
                Equipments = new Equipments
                {
                    Head = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.Head ?? 0),
                    Body = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.Body ?? 0),
                    RightHand = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.RightHand ?? 0),
                    LeftHand = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.LeftHand ?? 0),
                    Accessory1 = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.Accessory1 ?? 0),
                    Accessory2 = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.Accessory2 ?? 0)
                },
                EquippedDigievolutions = equippedDigievolutions
            };
        }

        private string GetDigimonNameById(DigimonAddresses addresses, byte id)
        {
            return addresses.Digimons?.FirstOrDefault(d => d.Id == id)?.Name ?? "Unknown";
        }

        public int GetDigimonBaseAddressById(DigimonAddresses addresses, byte id)
        {
            return (int)(addresses.Digimons?.FirstOrDefault(d => d.Id == id)?.Address ?? 0);
        }
    }
}
