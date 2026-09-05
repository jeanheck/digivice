import { type Digimon, type DigimonSlot, type Party } from "@/models";
import { Constant } from "@/constants/constant";
import { PartyHelper } from "@/helpers/party.helper";
import { EquipmentsHelper } from "@/presenters/helper/equipments.helper";
import { StatCapHelper } from "@/presenters/helper/stat-cap.helper";
import { EquipmentRepository } from "@/repositories/equipment.repository";

export class FooterPresenter {
  private static getDigimons(slots: DigimonSlot[]): Digimon[] {
    return slots.map((slot) => slot.digimon).filter((digimon) => digimon !== null);
  }

  public static getPartyLevel(party: Party): number {
    return PartyHelper.getLevel(party);
  }

  public static getPartyCharisma(digimonSlots: DigimonSlot[]): number {
    const digimons = this.getDigimons(digimonSlots);

    return Math.sum(
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
}
