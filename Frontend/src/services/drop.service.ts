import type { DropType } from "@/repositories/tables/raws/drop/drop-type";

export class DropService {
  public static getDropTranslationKeyById(dropId: number | string, type: DropType): string {
    const dropKey = String(dropId);

    if (type === "booster") {
      return `boosters.${dropKey}.name`;
    }

    if (type === "equipment") {
      return `equipments.${dropKey}.name`;
    }

    if (type === "consumableItem") {
      return `consumableItems.${dropKey}.name`;
    }

    return `drops.${dropKey}`;
  }
}
