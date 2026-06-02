import { Condition } from "@/constants/stat/condition";
import { ConditionDefinition } from "@/constants/stat/condition-definition";
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
        const definition = ConditionDefinition[conditionKey];

        if (definition.kind === "resistance") {
            const hasResistance = conditions[definition.canField];

            return {
                conditionKey,
                tooltipKey: hasResistance ? definition.resistanceTooltipKey : definition.canSufferTooltipKey,
                value: hasResistance ? `${conditions[definition.valueField]}%` : "No",
                icon: ConditionIcon[conditionKey],
                valueColorClass: hasResistance ? "text-white" : "text-red-400",
            };
        }

        const isEnabled = conditions[definition.canField];

        return {
            conditionKey,
            tooltipKey: definition.tooltipKey,
            value: isEnabled ? "Yes" : "No",
            icon: ConditionIcon[conditionKey],
            valueColorClass: isEnabled ? "text-green-400" : "text-red-400",
        };
    }
}
