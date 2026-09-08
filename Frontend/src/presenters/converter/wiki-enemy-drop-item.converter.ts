import type { EnemyDropViewModel } from "@/viewmodels/enemy/enemy-drop.viewmodel";
import type { WikiEnemyDropItemViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop-item.viewmodel";

export class WikiEnemyDropItemConverter {
  public static convert(drop: EnemyDropViewModel, labelKey: string): WikiEnemyDropItemViewModel {
    return {
      id: drop.id,
      labelKey,
      locationOnly: drop.locationOnly,
    };
  }
}
