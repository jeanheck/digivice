import type { DigimonDebuff } from "@/constants/digimon-debuff.constant";
import type { Enemy, Vital } from "@/models";
import { DigimonBattleConverter } from "@/presenters/converter/digimon-battle.converter";
import { EnemyRepository } from "@/repositories/enemy.repository";
import type { EnemyRaw } from "@/repositories/tables/raws/enemy/enemy.raw";
import { DigimonService } from "@/services/digimon.service";
import { EnemyAffinityService, type EnemyAffinities } from "@/services/enemy-affinity.service";
import type { DigimonStatus } from "@/types/digimon-status.type";
import type { DigimonBattleViewModel } from "@/viewmodels/map/digimon-battle.viewmodel";

export class DigimonBattlePresenter {
  public static getStatus(condition: number, hp: Vital): DigimonStatus {
    return DigimonService.getStatus(condition, hp);
  }

  public static getActiveDebuffs(condition: number): DigimonDebuff[] {
    return DigimonService.getActiveDebuffs(condition);
  }

  public static getViewModel(enemy: Enemy | null): DigimonBattleViewModel {
    const resolvedHp = enemy?.hp ?? { current: 0, max: 0 };
    const emptyAffinities: EnemyAffinities = { effective: [], notEffective: [] };

    if (enemy === null) {
      return DigimonBattleConverter.convert(null, resolvedHp, "", null, emptyAffinities);
    }

    const enemyRaw = EnemyRepository.getEnemyByMemoryIdAndGroupId(enemy.id, enemy.groupId);
    const enemyId = EnemyRepository.getEnemyIdByMemoryIdAndGroupId(enemy.id, enemy.groupId);
    const title = enemyRaw?.name ?? "";
    const affinities = DigimonBattlePresenter.getAffinities(enemyRaw) ?? emptyAffinities;

    return DigimonBattleConverter.convert(enemyRaw, resolvedHp, title, enemyId, affinities, {
      strength: enemy.strength,
      defense: enemy.defense,
      speed: enemy.speed,
    });
  }

  private static getAffinities(enemyRaw: EnemyRaw | null): EnemyAffinities | null {
    if (enemyRaw === null) {
      return null;
    }

    return EnemyAffinityService.getAffinities({
      fire: enemyRaw.fire,
      water: enemyRaw.water,
      ice: enemyRaw.ice,
      wind: enemyRaw.wind,
      thunder: enemyRaw.thunder,
      machine: enemyRaw.machine,
      dark: enemyRaw.dark,
    });
  }
}
