import { DropRepository } from "@/repositories/drop.repository";
import { EnemyRepository } from "@/repositories/enemy.repository";
import { EnemyConverter } from "@/presenters/converter/enemy.converter";
import { SearchItemConverter } from "@/presenters/converter/search-item.converter";
import type { DropSourceViewModel } from "@/viewmodels/drop/drop-source.viewmodel";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { SearchItemViewModel } from "@/viewmodels/search/search-item.viewmodel";

const VARIOUS_BOOSTER_DROP_ID = "variousBooster";

export class WikiModalPresenter {
  private static dropSourcesByDropId: Map<string, DropSourceViewModel[]> | null = null;

  public static getDropLabelKey(dropKey: string): string {
    const dropRaw = DropRepository.getDropByKey(dropKey);
    if (dropRaw === undefined) {
      return `drops.${dropKey}`;
    }

    if (dropRaw.type === "booster") {
      return `boosters.${dropRaw.id}.name`;
    }

    if (dropRaw.type === "equipment") {
      return `equipments.${dropRaw.id}.name`;
    }

    if (dropRaw.type === "consumableItem") {
      return `consumableItems.${dropRaw.id}.name`;
    }

    return `drops.${dropKey}`;
  }

  public static getEnemyById(enemyId: string): EnemyViewModel {
    const enemyRaw = EnemyRepository.getEnemyById(enemyId);
    return EnemyConverter.convert(enemyRaw);
  }

  public static getEnemySearchItems(): SearchItemViewModel[] {
    return Object.entries(EnemyRepository.getEnemyTable()).map(([enemyId, enemyRaw]) => {
      return SearchItemConverter.convertEnemy(enemyId, enemyRaw);
    });
  }

  public static getDropSearchItems(translateDropName: (dropKey: string) => string): SearchItemViewModel[] {
    return DropRepository.getDropKeys().map((dropKey) => {
      return SearchItemConverter.convertDrop(dropKey, translateDropName(dropKey));
    });
  }

  public static getAllSearchItems(translateDropName: (dropKey: string) => string): SearchItemViewModel[] {
    return [...this.getEnemySearchItems(), ...this.getDropSearchItems(translateDropName)];
  }

  public static getDropSources(dropId: string): DropSourceViewModel[] {
    return this.getDropSourcesByDropId().get(dropId) ?? [];
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
    return dropSourcesByDropId;
  }
}
