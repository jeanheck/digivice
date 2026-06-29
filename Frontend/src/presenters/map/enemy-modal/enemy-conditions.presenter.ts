import { EnemyConditionConverter } from "@/presenters/converter/enemy-condition.converter";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { EnemyConditionViewModel } from "@/viewmodels/enemy/enemy-condition.viewmodel";

export class EnemyConditionsPresenter {
    public static getConditions(conditions: EnemyViewModel["conditions"]): EnemyConditionViewModel[] {
        return EnemyConditionConverter.convertConditions(conditions);
    }
}
