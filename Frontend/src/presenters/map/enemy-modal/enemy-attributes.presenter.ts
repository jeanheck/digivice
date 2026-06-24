import { EnemyStatConverter } from "@/presenters/converter/enemy-stat.converter";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { EnemyStatViewModel } from "@/viewmodels/enemy/enemy-stat.viewmodel";

export class EnemyAttributesPresenter {
    public static getStats(attributes: EnemyViewModel["attributes"]): EnemyStatViewModel[] {
        return EnemyStatConverter.convertAttributes(attributes);
    }
}
