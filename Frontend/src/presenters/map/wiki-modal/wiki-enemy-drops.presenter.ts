import { DropRepository } from "@/repositories/drop.repository";
import { WikiEnemyDropItemConverter } from "@/presenters/converter/wiki-enemy-drop-item.converter";
import type { WikiEnemyDropViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop.viewmodel";
import type { WikiEnemyDropsViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drops.viewmodel";

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

    const hasInteractiveDrops = dropItems.length > 0;

    return {
      sectionLabelKey: hasInteractiveDrops && dropItems.length > 1 ? "enemy.drops" : "enemy.drop",
      fallbackLabelKey: "drops.none",
      hasInteractiveDrops,
      drops: dropItems,
    };
  }
}
