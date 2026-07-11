import { EnemyRepository } from "@/repositories/enemy.repository";
import { EnemyConverter } from "@/presenters/converter/enemy.converter";
import { SearchItemConverter } from "@/presenters/converter/search-item.converter";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { SearchItemViewModel } from "@/viewmodels/search/search-item.viewmodel";

export class EnemyModalPresenter {
  public static getEnemyById(enemyId: string): EnemyViewModel {
    const enemyRaw = EnemyRepository.getEnemyById(enemyId);
    return EnemyConverter.convert(enemyRaw);
  }

  public static getAllSearchItems(): SearchItemViewModel[] {
    return Object.entries(EnemyRepository.getEnemyTable()).map(([enemyId, enemyRaw]) => {
      return SearchItemConverter.convert(enemyId, enemyRaw);
    });
  }
}
