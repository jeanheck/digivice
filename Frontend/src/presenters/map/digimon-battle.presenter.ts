import type { Enemy } from "@/models/battle/enemy";
import { DigimonBattleConverter } from "@/presenters/converter/digimon-battle.converter";
import { EnemyRepository } from "@/repositories/enemy.repository";
import type { DigimonBattleViewModel } from "@/viewmodels/map/digimon-battle.viewmodel";

export class DigimonBattlePresenter {
  public static getViewModel(enemy: Enemy | null): DigimonBattleViewModel {
    const resolvedHp = enemy?.hp ?? { current: 0, max: 0 };

    if (enemy === null || enemy.id === 0) {
      return DigimonBattleConverter.convert(null, resolvedHp, "", null);
    }

    const enemyRaw = EnemyRepository.getEnemyByMemoryIdAndGroupId(enemy.id, enemy.groupId);
    const enemyId = EnemyRepository.getEnemyIdByMemoryIdAndGroupId(enemy.id, enemy.groupId);
    const title = enemyRaw?.name ?? "";

    return DigimonBattleConverter.convert(enemyRaw, resolvedHp, title, enemyId, {
      strength: enemy.strength,
      defense: enemy.defense,
      speed: enemy.speed,
    });
  }
}
