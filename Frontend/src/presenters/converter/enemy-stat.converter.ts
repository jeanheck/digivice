import { EnemyAttributeConstant } from "@/constants/stat/attribute/enemy-attribute.constant";
import { ElementConstant } from "@/constants/stat/element.constant";
import { IconConstant } from "@/constants/icon.constant";
import { Constant } from "@/constants/constant";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { EnemyStatViewModel } from "@/viewmodels/enemy/enemy-stat.viewmodel";

export class EnemyStatConverter {
    public static convertAttributes(attributes: EnemyViewModel["attributes"]): EnemyStatViewModel[] {
        return Object.values(EnemyAttributeConstant).map((statKey) => {
            return this.toStatViewModel(statKey, attributes[statKey]);
        });
    }

    public static convertElements(elements: EnemyViewModel["elements"]): EnemyStatViewModel[] {
        return Object.values(ElementConstant).map((statKey) => {
            return this.toStatViewModel(statKey, elements[statKey]);
        });
    }

    private static toStatViewModel(statKey: string, numericValue: number): EnemyStatViewModel {
        return {
            statKey,
            value: numericValue,
            icon: IconConstant[statKey as Constant],
        };
    }
}
