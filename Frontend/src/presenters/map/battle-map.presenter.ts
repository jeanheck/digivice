import type { Vital } from "@/models/party/digimon/vital";
import { BattleMapConverter } from "@/presenters/converter/battle-map.converter";
import { EnemyRepository } from "@/repositories/enemy.repository";
import type { BattleMapViewModel } from "@/viewmodels/map/battle-map.viewmodel";

export class BattleMapPresenter {
  private static readonly battleLocationId = "0600";

  public static isInBattle(locationId: string | null): boolean {
    return locationId === this.battleLocationId;
  }

  public static getViewModel(memoryId: number | null, hp: Vital | null): BattleMapViewModel {
    const resolvedHp = hp ?? { current: 0, max: 0 };
    if (memoryId === null) {
      return BattleMapConverter.convert(null, resolvedHp, "");
    }

    const enemyRaw = EnemyRepository.getEnemyByMemoryId(memoryId);
    const title = enemyRaw?.name ?? "";

    return BattleMapConverter.convert(enemyRaw, resolvedHp, title);
  }
}
