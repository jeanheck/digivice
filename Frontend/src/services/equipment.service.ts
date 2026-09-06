import type { Equipments } from "@/models";
import { Constant } from "@/constants/constant";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";

const TWO_HANDED_WEAPON_TYPE = "twoHandedWeapon";

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
      EquipmentRepository.getEquipmentById(equipments.right).type === TWO_HANDED_WEAPON_TYPE;

    if (!isTwoHandedWeapon) {
      pushIfEquipped(equipments.left);
    }

    pushIfEquipped(equipments.accessory1);
    pushIfEquipped(equipments.accessory2);

    return equipmentIds;
  }

  public static calculateBonus(stat: Constant, equipmentsRaws: EquipmentRaw[]): number {
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
