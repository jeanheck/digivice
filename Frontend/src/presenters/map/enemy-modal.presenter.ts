import { EnemyRepository } from "@/repositories/enemy.repository";
import { EnemyConverter } from "@/presenters/converter/enemy.converter";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

export class EnemyModalPresenter {
  public static getEnemyById(enemyId: string): EnemyViewModel {
    const enemyRaw = EnemyRepository.getEnemyById(enemyId);
    return EnemyConverter.convert(enemyRaw);
  }
}
