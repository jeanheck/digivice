import type { Party } from "@/state/models";
import { EquipmentRepository } from "@/presentation/repositories/equipment.repository";
import { EquipmentService } from "@/presentation/services/equipment.service";
import { StatService } from "@/presentation/services/stat.service";

export class PartyService {
  public static getLevel(party: Party): number {
    const digimons = party.slots.map((slot) => slot.digimon).filter((digimon) => digimon !== null);

    return Math.sum(
      digimons.map((digimon) => {
        return digimon.level;
      }),
    );
  }

  public static getCharisma(party: Party): number {
    const occupiedSlots = party.slots.filter((slot) => slot.digimon !== null);

    return Math.sum(
      occupiedSlots.map((slot) => {
        const digimon = slot.digimon!;
        const equipmentIds = EquipmentService.getEquipmentIds(digimon.equipments);
        const equipmentsRaws = EquipmentRepository.getEquipmentsByIds(equipmentIds);
        const equipmentSet = EquipmentService.getCompletedSet(slot.digimonId!, equipmentIds);
        const charismaBonus =
          EquipmentService.calculateBonus("charisma", equipmentsRaws) +
          EquipmentService.calculateSetBonus("charisma", equipmentSet);

        return StatService.calculateStat(digimon.attributes.charisma, charismaBonus);
      }),
    );
  }
}
