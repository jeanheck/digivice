import { EnemyAttributes } from "@/constants/digimon-attribute.constant";
import { DigimonElements } from "@/constants/digimon-element.constant";
import { DigimonStatIcon } from "@/constants/icon.constant";
import type { DigimonStat } from "@/types/digimon-stat.type";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { EnemyStatViewModel } from "@/viewmodels/enemy/enemy-stat.viewmodel";

export class EnemyStatConverter {
  public static convertAttributes(attributes: EnemyViewModel["attributes"]): EnemyStatViewModel[] {
    return EnemyAttributes.map((statKey) => {
      return this.toStatViewModel(statKey, attributes[statKey]);
    });
  }

  public static convertElements(elements: EnemyViewModel["elements"]): EnemyStatViewModel[] {
    return DigimonElements.map((statKey) => {
      return this.toStatViewModel(statKey, elements[statKey]);
    });
  }

  private static toStatViewModel(statKey: DigimonStat, numericValue: number): EnemyStatViewModel {
    return {
      statKey,
      value: numericValue,
      icon: DigimonStatIcon[statKey],
    };
  }
}
