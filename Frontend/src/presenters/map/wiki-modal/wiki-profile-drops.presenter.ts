import { DropRepository } from "@/repositories/drop.repository";
import { WikiProfileDropItemConverter } from "@/presenters/converter/wiki-profile-drop-item.converter";
import type { EnemyDropViewModel } from "@/viewmodels/enemy/enemy-drop.viewmodel";
import type { WikiProfileDropsViewModel } from "@/viewmodels/wiki-modal/wiki-profile-drops.viewmodel";

const VARIOUS_BOOSTER_DROP_ID = "variousBooster";

export class WikiProfileDropsPresenter {
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

  public static getViewModel(drops?: EnemyDropViewModel[]): WikiProfileDropsViewModel {
    const dropItems = (drops ?? []).map((drop) => {
      return WikiProfileDropItemConverter.convert(drop, this.getDropLabelKey(drop.id));
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
