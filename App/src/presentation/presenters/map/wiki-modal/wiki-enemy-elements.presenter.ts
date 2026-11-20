import { EnemyStatConverter } from "@/presentation/presenters/converter/enemy-stat.converter";
import type { EnemyViewModel } from "@/presentation/viewmodels/enemy/enemy.viewmodel";
import type { EnemyStatViewModel } from "@/presentation/viewmodels/enemy/enemy-stat.viewmodel";

export class WikiEnemyElementsPresenter {
  public static getStats(elements: EnemyViewModel["elements"]): EnemyStatViewModel[] {
    return EnemyStatConverter.convertElements(elements);
  }
}
