import { EnemyStatConverter } from "@/presenters/converter/enemy-stat.converter";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { EnemyStatViewModel } from "@/viewmodels/enemy/enemy-stat.viewmodel";

export class EnemyElementsPresenter {
    public static getStats(elements: EnemyViewModel["elements"]): EnemyStatViewModel[] {
        return EnemyStatConverter.convertElements(elements);
    }
}
