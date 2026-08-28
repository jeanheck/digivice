import type { Enemy } from "@/models/battle/enemy";
import { BattleMapConverter } from "@/presenters/converter/battle-map.converter";
import { EnemyRepository } from "@/repositories/enemy.repository";
import type { BattleMapViewModel } from "@/viewmodels/map/battle-map.viewmodel";

export class BattleMapPresenter {
  private static readonly battleLocationId = "0600";

  public static isInBattle(locationId: string | null): boolean {
    return locationId === this.battleLocationId;
  }

  public static getViewModel(enemy: Enemy | null): BattleMapViewModel {
    const resolvedHp = enemy?.hp ?? { current: 0, max: 0 };

    if (enemy === null || enemy.id === 0) {
      return BattleMapConverter.convert(null, resolvedHp, "", null);
    }

    const enemyRaw = EnemyRepository.getEnemyByMemoryIdAndGroupId(enemy.id, enemy.groupId);
    const enemyId = EnemyRepository.getEnemyIdByMemoryIdAndGroupId(enemy.id, enemy.groupId);
    const title = enemyRaw?.name ?? "";

    return BattleMapConverter.convert(enemyRaw, resolvedHp, title, enemyId, {
      strength: enemy.strength,
      defense: enemy.defense,
      speed: enemy.speed,
    });
  }
}
