import type { Equipments } from "@/models";
import { EquipmentRepository } from "@/repositories/equipment.repository";

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
}
