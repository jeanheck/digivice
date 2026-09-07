import type { WikiEnemyDropViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop.viewmodel";
import type { WikiEnemyDropItemViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop-item.viewmodel";

const VARIOUS_BOOSTER_DROP_ID = "variousBooster";

export class WikiEnemyDropItemConverter {
  public static convert(drop: WikiEnemyDropViewModel, labelKey: string): WikiEnemyDropItemViewModel {
    return {
      id: drop.id,
      labelKey,
      locationOnly: drop.locationOnly,
      isClickable: drop.id !== VARIOUS_BOOSTER_DROP_ID,
    };
  }
}
