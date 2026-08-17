import type { DropSourceViewModel } from "@/viewmodels/drop/drop-source.viewmodel";
import type { WikiDroppedByEnemyViewModel } from "@/viewmodels/wiki-modal/wiki-dropped-by-enemy.viewmodel";

export class WikiDroppedByEnemyConverter {
  public static convert(
    dropSource: DropSourceViewModel,
    iconUrl: string | null,
  ): WikiDroppedByEnemyViewModel {
    return {
      enemyId: dropSource.enemyId,
      enemyName: dropSource.enemyName,
      iconUrl,
      locationOnly: dropSource.locationOnly,
    };
  }
}
