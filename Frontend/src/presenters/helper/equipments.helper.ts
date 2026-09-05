import { Constant } from "@/constants/constant";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";

export class EquipmentsHelper {
  public static calculateBonus(
    stat: Constant,
    equipmentsRaws: EquipmentRaw[],
  ): number {
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
