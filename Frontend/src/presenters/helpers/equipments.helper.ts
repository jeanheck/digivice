import type { Equipments } from "@/models";

export class EquipmentsHelper {
  public static getEquipmentIds(equipments: Equipments): number[] {
    return [
      equipments.head,
      equipments.body,
      equipments.right,
      equipments.left,
      equipments.accessory1,
      equipments.accessory2,
    ].filter((id): id is number => id !== null && id !== undefined && id !== 0);
  }

  public static getUniqueEquipmentIds(equipments: Equipments): number[] {
    const equipmentIds = EquipmentsHelper.getEquipmentIds(equipments);

    return [...new Set(equipmentIds)];
  }
}
