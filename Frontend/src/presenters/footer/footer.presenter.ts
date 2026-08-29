import { type Digimon, type DigimonSlot } from "@/models";
import { Constant } from "@/constants/constant";
import { EquipmentsHelper } from "@/presenters/helper/equipments.helper";
import { MathHelper } from "@/presenters/helper/math.helper";
import { StatCapHelper } from "@/presenters/helper/stat-cap.helper";
import { EquipmentRepository } from "@/repositories/equipment.repository";

export class FooterPresenter {
  private static getDigimons(slots: DigimonSlot[]): Digimon[] {
    return slots.map((slot) => slot.digimon).filter((digimon) => digimon !== null);
  }

  public static getPartyCharisma(digimonSlots: DigimonSlot[]): number {
    const digimons = this.getDigimons(digimonSlots);

    return MathHelper.sum(
      digimons.map((digimon) => {
        const equipmentIds = EquipmentsHelper.getBonusCalculationEquipmentIds(
          digimon.equipments,
          (equipmentId) => EquipmentRepository.getEquipmentById(equipmentId).type,
        );
        const rawEquipments = EquipmentRepository.getEquipmentsByIds(equipmentIds);
        const charismaEquipBonus = EquipmentsHelper.calculateBonusFromEquipaments(
          Constant.charisma,
          rawEquipments,
        );

        return StatCapHelper.capBasePlusEquip(digimon.attributes.charisma, charismaEquipBonus);
      }),
    );
  }

  public static getPartyLevel(digimonSlots: DigimonSlot[]): number {
    const digimons = this.getDigimons(digimonSlots);

    return MathHelper.sum(
      digimons.map((digimon) => {
        return digimon.level;
      }),
    );
  }
}
