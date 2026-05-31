import { EnemyRepository } from "@/repositories/enemy.repository";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

export class EnemyModalPresenter {
  public static getEnemyById(enemyId: string): EnemyViewModel {
    return EnemyRepository.getEnemyById(enemyId);
  }
}
