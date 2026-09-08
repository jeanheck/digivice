import { WikiEnemyDropItemConverter } from "@/presenters/converter/wiki-enemy-drop-item.converter";
import { DropService } from "@/services/drop.service";
import type { EnemyDropViewModel } from "@/viewmodels/enemy/enemy-drop.viewmodel";
import type { WikiEnemyDropItemViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop-item.viewmodel";

export class WikiEnemyDropsPresenter {
  public static getViewModel(drops?: EnemyDropViewModel[]): WikiEnemyDropItemViewModel[] {
    return (drops ?? []).map((drop) => {
      return WikiEnemyDropItemConverter.convert(
        drop,
        DropService.getDropTranslationKeyById(drop.id),
      );
    });
  }
}
