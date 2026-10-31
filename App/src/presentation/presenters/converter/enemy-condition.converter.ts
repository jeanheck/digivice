import {
  DigimonConditions,
  type DigimonCondition,
} from "@/shared/constants/digimon-condition.constant";
import { DigimonConditionIcon } from "@/shared/constants/icon.constant";
import type { EnemyConditionViewModel } from "@/presentation/viewmodels/enemy/enemy-condition.viewmodel";
import type { EnemyViewModel } from "@/presentation/viewmodels/enemy/enemy.viewmodel";
import { ImageCatalog } from "@/ui/catalogs/image.catalog";

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
        iconUrl: ImageCatalog.getConditionImageUrl(conditionKey),
        value: condition.can ? condition.value.toString() : "",
      };
    }

    return {
      conditionKey,
      can: condition.can,
      icon: DigimonConditionIcon[conditionKey],
      iconUrl: ImageCatalog.getConditionImageUrl(conditionKey),
    };
  }
}
