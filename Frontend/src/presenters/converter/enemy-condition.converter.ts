import { DigimonConditions, type DigimonCondition } from "@/constants/digimon-condition.constant";
import { DigimonConditionIcon } from "@/constants/icon.constant";
import type { EnemyConditionViewModel } from "@/viewmodels/enemy/enemy-condition.viewmodel";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

export class EnemyConditionConverter {
  public static convertConditions(
    conditions: EnemyViewModel["conditions"],
  ): EnemyConditionViewModel[] {
    return DigimonConditions.map((conditionKey) => {
      return EnemyConditionConverter.toConditionViewModel(conditionKey, conditions);
    });
  }

  private static toConditionViewModel(
    conditionKey: DigimonCondition,
    conditions: EnemyViewModel["conditions"],
  ): EnemyConditionViewModel {
    const condition = conditions[conditionKey];

    if ("value" in condition) {
      return {
        conditionKey,
        can: condition.can,
        icon: DigimonConditionIcon[conditionKey],
        value: condition.can ? condition.value.toString() : "",
      };
    }

    return {
      conditionKey,
      can: condition.can,
      icon: DigimonConditionIcon[conditionKey],
    };
  }
}
