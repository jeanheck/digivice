import { EnemyStatConverter } from "@/presentation/presenters/converter/enemy-stat.converter";
import type { EnemyViewModel } from "@/presentation/viewmodels/enemy/enemy.viewmodel";
import type { EnemyStatViewModel } from "@/presentation/viewmodels/enemy/enemy-stat.viewmodel";

export class WikiEnemyAttributesPresenter {
  public static getStats(attributes: EnemyViewModel["attributes"]): EnemyStatViewModel[] {
    return EnemyStatConverter.convertAttributes(attributes);
  }
}
