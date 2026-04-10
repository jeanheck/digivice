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
                    Experience = MemoryUtils.ReadInt32FromBlock(resource.LogicBlock, addresses.BasicInfo?.Experience),
                    Level = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.Level),
                    CurrentHP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.CurrentHP),
                    MaxHP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.MaxHP),
                    CurrentMP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.CurrentMP),
                    MaxMP = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.BasicInfo?.MaxMP)
                },
                Attributes = new Attributes
                {
                    Strength = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Strength),
                    Defense = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Defense),
                    Spirit = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Spirit),
                    Wisdom = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Wisdow),
                    Speed = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Speed),
                    Charisma = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Attributes?.Charisma)
                },
                Resistances = new Resistances
                {
                    Fire = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Fire),
                    Water = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Water),
                    Ice = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Ice),
                    Wind = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Wind),
                    Thunder = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Thunder),
                    Machine = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Machine),
                    Dark = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Resistances?.Dark)
                },
                Equipments = new Equipments
                {
                    Head = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.Head),
                    Body = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.Body),
                    RightHand = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.RightHand),
                    LeftHand = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.LeftHand),
                    Accessory1 = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.Accessory1),
                    Accessory2 = MemoryUtils.ReadInt16FromBlock(resource.LogicBlock, addresses.Equipaments?.Accessory2)
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
            var addressHex = addresses.Digimons?.FirstOrDefault(d => d.Id == id)?.Address;
            return MemoryUtils.ParseHex(addressHex);
        }
    }
}
