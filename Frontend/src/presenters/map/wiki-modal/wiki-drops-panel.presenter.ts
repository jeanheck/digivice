import { ImageCatalog } from "@/catalogs/image.catalog";
import { WikiDroppedByEnemyConverter } from "@/presenters/converter/wiki-dropped-by-enemy.converter";
import { DropRepository } from "@/repositories/drop.repository";
import { EnemyRepository } from "@/repositories/enemy.repository";
import type { DropSourceViewModel } from "@/viewmodels/drop/drop-source.viewmodel";
import type { WikiDropsPanelViewModel } from "@/viewmodels/wiki-modal/wiki-drops-panel.viewmodel";

const VARIOUS_BOOSTER_DROP_ID = "variousBooster";

export class WikiDropsPanelPresenter {
  private static dropSourcesByDropId: Map<string, DropSourceViewModel[]> | null = null;

  public static getViewModel(dropId: string): WikiDropsPanelViewModel {
    const dropRaw = DropRepository.getDropByKey(dropId);
    const dropSources = WikiDropsPanelPresenter.getDropSourcesByDropId().get(dropId) ?? [];

    return {
      dropType: dropRaw?.type ?? null,
      dropNumericId: dropRaw?.id ?? null,
      sources: dropSources.map((dropSource) => {
        return WikiDroppedByEnemyConverter.convert(
          dropSource,
          ImageCatalog.getEnemyIconUrl(dropSource.enemyName),
        );
      }),
    };
  }

  private static getDropSourcesByDropId(): Map<string, DropSourceViewModel[]> {
    if (this.dropSourcesByDropId !== null) {
      return this.dropSourcesByDropId;
    }

    const dropSourcesByDropId = new Map<string, DropSourceViewModel[]>();

    for (const [enemyId, enemyRaw] of Object.entries(EnemyRepository.getEnemyTable())) {
      for (const drop of enemyRaw.drops ?? []) {
        if (drop.id === VARIOUS_BOOSTER_DROP_ID) {
          continue;
        }

        const existingSources = dropSourcesByDropId.get(drop.id) ?? [];
        existingSources.push({
          enemyId,
          enemyName: enemyRaw.name,
          locationOnly: drop.locationOnly,
        });
        dropSourcesByDropId.set(drop.id, existingSources);
      }
    }

    this.dropSourcesByDropId = dropSourcesByDropId;
    return this.dropSourcesByDropId;
  }
}
