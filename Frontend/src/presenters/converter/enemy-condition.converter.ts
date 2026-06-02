import { Condition } from "@/constants/stat/condition";
import { ConditionIcon } from "@/constants/stat/condition-icon";
import type { EnemyConditionViewModel } from "@/viewmodels/enemy/enemy-condition.viewmodel";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

export class EnemyConditionConverter {
    public static convertConditions(conditions: EnemyViewModel["conditions"]): EnemyConditionViewModel[] {
        return Object.values(Condition).map((conditionKey) => {
            return EnemyConditionConverter.toConditionViewModel(conditionKey, conditions);
        });
    }

    private static toConditionViewModel(
        conditionKey: Condition,
        conditions: EnemyViewModel["conditions"]
    ): EnemyConditionViewModel {
        const condition = conditions[conditionKey];

        if ("value" in condition) {
            return {
                conditionKey,
                can: condition.can,
                icon: ConditionIcon[conditionKey],
                value: condition.can ? `${condition.value}%` : "",
            };
        }

        return {
            conditionKey,
            can: condition.can,
            icon: ConditionIcon[conditionKey]
        };
    }
}
