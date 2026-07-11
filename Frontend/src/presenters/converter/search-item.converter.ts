import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";
import type { SearchItemViewModel } from "@/viewmodels/search/search-item.viewmodel";

export class SearchItemConverter {
  public static convert(id: string, enemyRaw: EnemyRaw): SearchItemViewModel {
    return {
      id,
      name: enemyRaw.name,
    };
  }
}
