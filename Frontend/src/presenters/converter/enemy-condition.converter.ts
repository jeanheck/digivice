import { ConditionConstant } from "@/constants/stat/condition.constant";
import { IconConstant } from "@/constants/icon.constant";
import type { EnemyConditionViewModel } from "@/viewmodels/enemy/enemy-condition.viewmodel";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

export class EnemyConditionConverter {
    public static convertConditions(conditions: EnemyViewModel["conditions"]): EnemyConditionViewModel[] {
        return Object.values(ConditionConstant).map((conditionKey) => {
            return EnemyConditionConverter.toConditionViewModel(conditionKey, conditions);
        });
    }

    private static toConditionViewModel(
        conditionKey: ConditionConstant,
        conditions: EnemyViewModel["conditions"]
    ): EnemyConditionViewModel {
        const condition = conditions[conditionKey];

        if ("value" in condition) {
            return {
                conditionKey,
                can: condition.can,
                icon: IconConstant[conditionKey],
                value: condition.can ? `${condition.value}%` : "",
            };
        }

        return {
            conditionKey,
            can: condition.can,
            icon: IconConstant[conditionKey]
        };
    }
}
