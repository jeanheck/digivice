import type { EnemyDropViewModel } from "@/viewmodels/enemy/enemy-drop.viewmodel";
import type { WikiEnemyDropViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop.viewmodel";

export class WikiEnemyDropConverter {
  public static convert(drop: EnemyDropViewModel, labelKey: string): WikiEnemyDropViewModel {
    return {
      id: drop.id,
      labelKey,
      locationOnly: drop.locationOnly,
    };
  }
}
