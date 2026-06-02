import { Condition } from "@/constants/stat/condition";
import { ConditionIcon } from "@/constants/stat/condition-icon";
import type { EnemyConditionViewModel } from "@/viewmodels/enemy/enemy-condition.viewmodel";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

type EnemyResistanceCondition = {
    can: boolean;
    value: number;
};

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
            const resistanceCondition = condition as EnemyResistanceCondition;

            return {
                conditionKey,
                can: resistanceCondition.can,
                icon: ConditionIcon[conditionKey],
                value: resistanceCondition.can ? `${resistanceCondition.value}%` : null,
            };
        }

        return {
            conditionKey,
            can: condition.can,
            icon: ConditionIcon[conditionKey],
            value: null,
        };
    }
}
