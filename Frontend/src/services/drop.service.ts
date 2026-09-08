import { DropRepository } from "@/repositories/drop.repository";

export class DropService {
  public static getDropTranslationKeyById(dropKey: string): string {
    const dropRaw = DropRepository.getDropByKey(dropKey);
    if (dropRaw === undefined) {
      return `drops.${dropKey}`;
    }

    if (dropRaw.type === "booster") {
      return `boosters.${dropKey}.name`;
    }

    if (dropRaw.type === "equipment") {
      return `equipments.${dropKey}.name`;
    }

    if (dropRaw.type === "consumableItem") {
      return `consumableItems.${dropKey}.name`;
    }

    return `drops.${dropKey}`;
  }
}
