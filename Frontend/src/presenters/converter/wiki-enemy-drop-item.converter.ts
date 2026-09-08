import type { WikiEnemyDropViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop.viewmodel";
import type { WikiEnemyDropItemViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop-item.viewmodel";

export class WikiEnemyDropItemConverter {
  public static convert(drop: WikiEnemyDropViewModel, labelKey: string): WikiEnemyDropItemViewModel {
    return {
      id: drop.id,
      labelKey,
      locationOnly: drop.locationOnly,
    };
  }
}
