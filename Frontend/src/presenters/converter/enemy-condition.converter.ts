import { Condition } from "@/constants/stat/condition";
import { ConditionDefinition } from "@/constants/stat/condition-definition";
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
        const definition = ConditionDefinition[conditionKey];
        const condition = conditions[conditionKey];

        if (definition.kind === "boolean") {
            const isEnabled = condition.can;

            return {
                conditionKey,
                tooltipKey: definition.tooltipKey,
                value: isEnabled ? "Yes" : "No",
                icon: ConditionIcon[conditionKey],
                valueColorClass: isEnabled ? "text-green-400" : "text-red-400",
            };
        }

        const resistanceCondition = condition as EnemyResistanceCondition;
        const hasResistance = resistanceCondition.can;

        return {
            conditionKey,
            tooltipKey: hasResistance ? definition.resistanceTooltipKey : definition.canSufferTooltipKey,
            value: hasResistance ? `${resistanceCondition.value}%` : "No",
            icon: ConditionIcon[conditionKey],
            valueColorClass: hasResistance ? "text-white" : "text-red-400",
        };
    }
}
