import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";
import type { SearchItemViewModel } from "@/viewmodels/search/search-item.viewmodel";

export interface EnemySearchItemLabels {
  translateTamerName: (tamerId: string) => string;
  translateNpcName: (npcId: string) => string;
}

export class SearchItemConverter {
  public static convertEnemy(
    id: string,
    enemyRaw: EnemyRaw,
    labels: EnemySearchItemLabels,
  ): SearchItemViewModel {
    const searchItem: SearchItemViewModel = {
      id,
      name: enemyRaw.name,
      kind: "enemy",
    };
    const levelLabel = String(enemyRaw.level);

    if (enemyRaw.boss === true) {
      searchItem.kindLabelKey = "enemy.searchContext.boss";
      searchItem.kindLabelParams = { level: levelLabel };
      return searchItem;
    }

    if (enemyRaw.tamerId !== undefined) {
      searchItem.kindLabelKey = "enemy.searchContext.tamer";
      searchItem.kindLabelParams = {
        name: labels.translateTamerName(enemyRaw.tamerId),
        level: levelLabel,
      };
      return searchItem;
    }

    if (enemyRaw.npcId !== undefined) {
      searchItem.kindLabelKey = "enemy.searchContext.npc";
      searchItem.kindLabelParams = {
        name: labels.translateNpcName(enemyRaw.npcId),
        level: levelLabel,
      };
      return searchItem;
    }

    searchItem.kindLabelKey = "enemy.searchContext.wild";
    searchItem.kindLabelParams = { level: levelLabel };
    return searchItem;
  }

  public static convertDrop(id: string, name: string): SearchItemViewModel {
    return {
      id,
      name,
      kind: "drop",
    };
  }

  public static convertCard(id: string, name: string): SearchItemViewModel {
    return {
      id,
      name,
      kind: "card",
    };
  }

  public static convertLocation(id: string, name: string): SearchItemViewModel {
    return {
      id,
      name,
      kind: "location",
    };
  }

  public static convertStore(id: string, name: string): SearchItemViewModel {
    return {
      id,
      name,
      kind: "store",
    };
  }

  public static convertTamer(id: string, translatedName: string): SearchItemViewModel {
    return {
      id,
      name: translatedName,
      kind: "tamer",
    };
  }

  public static convertDuelIsland(id: string, translatedName: string): SearchItemViewModel {
    return {
      id,
      name: translatedName,
      kind: "npc",
    };
  }

  public static convertStoryNpc(
    id: string,
    translatedName: string,
    kind: "leader" | "npc",
  ): SearchItemViewModel {
    return {
      id,
      name: translatedName,
      kind,
    };
  }
}
