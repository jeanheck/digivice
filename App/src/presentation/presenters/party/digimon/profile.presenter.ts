import type { DigimonDebuff } from "@/shared/constants/digimon-debuff.constant";
import type { Digimon, InBattle, Vital } from "@/state/models";
import { DigimonRepository } from "@/presentation/repositories/digimon.repository";
import { DigimonService } from "@/presentation/services/digimon.service";
import { DigimonBattleSelector } from "@/state/stores/selectors/digimon-battle.selector";
import type { DigimonStatus } from "@/shared/types/digimon-status.type";

export class ProfilePresenter {
  public static isInBattle(location: string, inBattle: InBattle): boolean {
    return DigimonBattleSelector.isInBattle(location, inBattle);
  }

  public static getHp(digimon: Digimon, isInBattle: boolean): Vital {
    return isInBattle ? digimon.inBattle.hp : digimon.hp;
  }

  public static getMp(digimon: Digimon, isInBattle: boolean): Vital {
    return isInBattle ? digimon.inBattle.mp : digimon.mp;
  }

  public static getCondition(digimon: Digimon, isInBattle: boolean): number {
    return isInBattle ? digimon.inBattle.condition : 0;
  }

  public static getName(id: number): string {
    return DigimonRepository.getNameById(id);
  }

  public static getStatus(condition: number, hp: Vital): DigimonStatus {
    return DigimonService.getStatus(condition, hp);
  }

  public static getActiveDebuffs(condition: number): DigimonDebuff[] {
    return DigimonService.getActiveDebuffs(condition);
  }
}
