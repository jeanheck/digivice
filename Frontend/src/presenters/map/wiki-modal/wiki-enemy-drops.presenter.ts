import { WikiEnemyDropConverter } from "@/presenters/converter/wiki-enemy-drop.converter";
import { DropService } from "@/services/drop.service";
import type { EnemyDropViewModel } from "@/viewmodels/enemy/enemy-drop.viewmodel";
import type { WikiEnemyDropViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop.viewmodel";

export class WikiEnemyDropsPresenter {
  public static getViewModel(drops?: EnemyDropViewModel[]): WikiEnemyDropViewModel[] {
    return (drops ?? []).map((drop) => {
      return WikiEnemyDropConverter.convert(
        drop,
        DropService.getDropTranslationKeyById(String(drop.dropId)),
      );
    });
  }
}
