import { EnemyAttribute } from "@/constants/stat/attribute/enemy-attribute";
import { Element } from "@/constants/stat/element";
import { StatIcon } from "@/constants/stat/stat-icon";
import { StatKey } from "@/constants/stat/stat-key";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { EnemyStatViewModel } from "@/viewmodels/enemy/enemy-stat.viewmodel";

export class EnemyStatConverter {
    public static convertAttributes(attributes: EnemyViewModel["attributes"]): EnemyStatViewModel[] {
        return Object.values(EnemyAttribute).map((statKey) => {
            return EnemyStatConverter.toStatViewModel(statKey, attributes[statKey]);
        });
    }

    public static convertElements(elements: EnemyViewModel["elements"]): EnemyStatViewModel[] {
        return Object.values(Element).map((statKey) => {
            return EnemyStatConverter.toStatViewModel(statKey, elements[statKey]);
        });
    }

    private static toStatViewModel(statKey: string, numericValue: number): EnemyStatViewModel {
        return {
            statKey,
            value: String(numericValue),
            icon: StatIcon[statKey as StatKey],
        };
    }
}
