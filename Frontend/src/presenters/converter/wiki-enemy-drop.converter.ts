import type { EnemyDropViewModel } from "@/viewmodels/enemy/enemy-drop.viewmodel";
import type { WikiEnemyDropViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop.viewmodel";

export class WikiEnemyDropConverter {
  public static convert(drop: EnemyDropViewModel, labelKey: string): WikiEnemyDropViewModel {
    return {
      dropId: drop.dropId,
      type: drop.type,
      labelKey,
      locationOnly: drop.locationOnly,
    };
  }
}
