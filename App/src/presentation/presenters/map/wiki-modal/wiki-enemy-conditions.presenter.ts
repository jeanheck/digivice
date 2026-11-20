import { EnemyConditionConverter } from "@/presentation/presenters/converter/enemy-condition.converter";
import type { EnemyViewModel } from "@/presentation/viewmodels/enemy/enemy.viewmodel";
import type { EnemyConditionViewModel } from "@/presentation/viewmodels/enemy/enemy-condition.viewmodel";

export class WikiEnemyConditionsPresenter {
  public static getConditions(conditions: EnemyViewModel["conditions"]): EnemyConditionViewModel[] {
    return EnemyConditionConverter.convertConditions(conditions);
  }
}
