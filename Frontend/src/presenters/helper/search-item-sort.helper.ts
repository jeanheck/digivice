import type { SearchItemKind, SearchItemViewModel } from "@/viewmodels/search/search-item.viewmodel";

const WILD_ENEMY_CONTEXT_KEY = "enemy.searchContext.wild";

export class SearchItemSortHelper {
  private static getGroupPriority(item: SearchItemViewModel): number {
    if (item.kindLabelKey === WILD_ENEMY_CONTEXT_KEY) {
      return 0;
    }

    if (item.kind === "card") {
      return 1;
    }

    if (item.kind === "enemy") {
      return 2;
    }

    return 3;
  }

  private static getEnemyLevel(item: SearchItemViewModel): number {
    const level = item.kindLabelParams?.level;
    if (level === undefined) {
      return 0;
    }

    return Number(level);
  }

  private static getOtherKindOrder(kind: SearchItemKind | undefined): number {
    if (kind === "drop") {
      return 0;
    }

    if (kind === "location") {
      return 1;
    }

    if (kind === "store") {
      return 2;
    }

    if (kind === "tamer") {
      return 3;
    }

    if (kind === "leader") {
      return 4;
    }

    if (kind === "npc") {
      return 5;
    }

    return 99;
  }

  private static compareItems(
    firstItem: SearchItemViewModel,
    secondItem: SearchItemViewModel,
  ): number {
    const byName = firstItem.name.localeCompare(secondItem.name);
    if (byName !== 0) {
      return byName;
    }

    const firstGroup = this.getGroupPriority(firstItem);
    const secondGroup = this.getGroupPriority(secondItem);
    if (firstGroup !== secondGroup) {
      return firstGroup - secondGroup;
    }

    if (firstGroup === 2) {
      const byLevel = this.getEnemyLevel(firstItem) - this.getEnemyLevel(secondItem);
      if (byLevel !== 0) {
        return byLevel;
      }
    }

    if (firstGroup === 3) {
      const byKind =
        this.getOtherKindOrder(firstItem.kind) - this.getOtherKindOrder(secondItem.kind);
      if (byKind !== 0) {
        return byKind;
      }
    }

    return firstItem.id.localeCompare(secondItem.id);
  }

  public static sort(items: SearchItemViewModel[]): SearchItemViewModel[] {
    return [...items].sort((firstItem, secondItem) => {
      return this.compareItems(firstItem, secondItem);
    });
  }
}
