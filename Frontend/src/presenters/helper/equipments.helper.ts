import type { Equipments } from "@/models";
import { StatKey } from "@/constants/stat/stat-key";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import { MathUtils } from "@/utils/MathUtils";

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

  public static calculateBonusFromEquipaments(stat: StatKey, rawEquipments: EquipmentRaw[]): number {
    const lowerCaseType = stat.toLowerCase();
    const attributesRaw = rawEquipments
      .flatMap((rawEquipment) => rawEquipment.attributes)
      .filter((attribute) => attribute.attribute.toLowerCase() === lowerCaseType);

    return MathUtils.Sum(attributesRaw.map((attribute) => {
      return Number(`${attribute.type}${attribute.value}`);
    }));
  }
}
