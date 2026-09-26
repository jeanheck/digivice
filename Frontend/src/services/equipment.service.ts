import type { Equipments } from "@/models";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import type { DigimonStat } from "@/types/digimon-stat.type";

const TwoHandedWeaponType = "twoHandedWeapon";

export class EquipmentService {
  public static getEquipmentIds(equipments: Equipments): number[] {
    const equipmentIds: number[] = [];

    const pushIfEquipped = (equipmentId: number | null): void => {
      if (equipmentId !== null) {
        equipmentIds.push(equipmentId);
      }
    };

    pushIfEquipped(equipments.head);
    pushIfEquipped(equipments.body);
    pushIfEquipped(equipments.right);

    const isTwoHandedWeapon =
      equipments.right !== null &&
      EquipmentRepository.getEquipmentById(equipments.right).type === TwoHandedWeaponType;

    if (!isTwoHandedWeapon) {
      pushIfEquipped(equipments.left);
    }

    pushIfEquipped(equipments.accessory1);
    pushIfEquipped(equipments.accessory2);

    return equipmentIds;
  }

  public static calculateBonus(stat: DigimonStat, equipmentsRaws: EquipmentRaw[]): number {
    const equipmentAttributeRaws = equipmentsRaws
      .flatMap((equipmentRaw) => equipmentRaw.attributes)
      .filter((equipmentAttributeRaw) => equipmentAttributeRaw.attribute === stat);

    return Math.sum(
      equipmentAttributeRaws.map((equipmentAttributeRaw) => {
        return Number(`${equipmentAttributeRaw.type}${equipmentAttributeRaw.value}`);
      }),
    );
  }
}
