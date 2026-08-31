import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";
import type { TamerRaw } from "@/repositories/tables/raws/tamer/tamer.raw";
import type { SearchItemViewModel } from "@/viewmodels/search/search-item.viewmodel";

export class SearchItemConverter {
  public static convertEnemy(id: string, enemyRaw: EnemyRaw): SearchItemViewModel {
    return {
      id,
      name: enemyRaw.name,
      kind: "enemy",
    };
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

  public static convertTamer(
    id: string,
    translatedName: string,
    tamerRaw: TamerRaw,
  ): SearchItemViewModel {
    return {
      id,
      name: translatedName,
      kind: tamerRaw.type,
    };
  }
}
