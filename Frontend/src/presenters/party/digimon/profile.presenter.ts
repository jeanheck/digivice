import { DigimonStatusConstant } from "@/constants/digimon-status.constant";
import type { Digimon, InBattle, Vital } from "@/models";
import { DigimonRepository } from "@/repositories/digimon.repository";
import { DigimonService } from "@/services/digimon.service";
import { DigimonBattleSelector } from "@/stores/selectors/digimon-battle.selector";

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

  public static getStatus(condition: number, hp: Vital): DigimonStatusConstant {
    return DigimonService.getStatus(condition, hp);
  }
}
