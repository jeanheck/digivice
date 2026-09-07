import { DropRepository } from "@/repositories/drop.repository";
import { WikiEnemyDropItemConverter } from "@/presenters/converter/wiki-enemy-drop-item.converter";
import type { WikiEnemyDropViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop.viewmodel";
import type { WikiEnemyDropsViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drops.viewmodel";

const VARIOUS_BOOSTER_DROP_ID = "variousBooster";

export class WikiEnemyDropsPresenter {
  public static getDropLabelKey(dropKey: string): string {
    const dropRaw = DropRepository.getDropByKey(dropKey);
    if (dropRaw === undefined) {
      return `drops.${dropKey}`;
    }

    if (dropRaw.type === "booster") {
      return `boosters.${dropRaw.id}.name`;
    }

    if (dropRaw.type === "equipment") {
      return `equipments.${dropRaw.id}.name`;
    }

    if (dropRaw.type === "consumableItem") {
      return `consumableItems.${dropRaw.id}.name`;
    }

    return `drops.${dropKey}`;
  }

  public static getViewModel(drops?: WikiEnemyDropViewModel[]): WikiEnemyDropsViewModel {
    const dropItems = (drops ?? []).map((drop) => {
      return WikiEnemyDropItemConverter.convert(drop, this.getDropLabelKey(drop.id));
    });

    const isVariousBoosterOnly =
      dropItems.length === 1 && dropItems[0]?.id === VARIOUS_BOOSTER_DROP_ID;
    const hasInteractiveDrops = dropItems.length > 0 && !isVariousBoosterOnly;

    return {
      sectionLabelKey: hasInteractiveDrops && dropItems.length > 1 ? "enemy.drops" : "enemy.drop",
      fallbackLabelKey: isVariousBoosterOnly ? "drops.variousBooster" : "drops.none",
      hasInteractiveDrops,
      drops: dropItems,
    };
  }
}
