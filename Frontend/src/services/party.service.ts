import type { Party } from "@/models";
import { Constant } from "@/constants/constant";
import { EquipmentsHelper } from "@/helpers/equipments.helper";
import { StatHelper } from "@/helpers/stat.helper";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import { EquipmentService } from "@/services/equipment.service";

export class PartyService {
  public static getCharisma(party: Party): number {
    const digimons = party.slots
      .map((slot) => slot.digimon)
      .filter((digimon) => digimon !== null);

    return Math.sum(
      digimons.map((digimon) => {
        const equipmentIds = EquipmentService.getEquipmentIds(digimon.equipments);
        const equipmentsRaws = EquipmentRepository.getEquipmentsByIds(equipmentIds);
        const charismaBonus = EquipmentsHelper.calculateBonus(
          Constant.charisma,
          equipmentsRaws,
        );

        return StatHelper.calculateStat(digimon.attributes.charisma, charismaBonus);
      }),
    );
  }
}
