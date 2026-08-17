import type { EnemyDropViewModel } from "@/viewmodels/enemy/enemy-drop.viewmodel";
import type { WikiProfileDropItemViewModel } from "@/viewmodels/wiki-modal/wiki-profile-drop-item.viewmodel";

const VARIOUS_BOOSTER_DROP_ID = "variousBooster";

export class WikiProfileDropItemConverter {
  public static convert(drop: EnemyDropViewModel, labelKey: string): WikiProfileDropItemViewModel {
    return {
      id: drop.id,
      labelKey,
      locationOnly: drop.locationOnly,
      isClickable: drop.id !== VARIOUS_BOOSTER_DROP_ID,
    };
  }
}
