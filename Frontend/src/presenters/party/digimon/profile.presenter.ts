import { DigimonConditionConstant } from "@/constants/digimon-condition.constant";
import type { Digimon } from "@/models/party/digimon/digimon";
import type { InBattle } from "@/models/party/digimon/in-battle";
import type { Vital } from "@/models/party/digimon/vital";
import { DigimonRepository } from "@/repositories/digimon.repository";

export class ProfilePresenter {
  private static readonly battleLocationId = "0600";

  public static isInBattle(location: string | null, inBattle: InBattle): boolean {
    return location === this.battleLocationId && inBattle.hp.max !== 0;
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

  public static getNameById(id: number): string {
    return DigimonRepository.getNameById(id);
  }

  public static getCalculatedCondition(
    condition: number,
    hp: Vital,
  ): DigimonConditionConstant {
    if (hp.current === 0) {
      return DigimonConditionConstant.ko;
    }
    if (condition !== 0) {
      return DigimonConditionConstant.condition;
    }
    if (hp.current < hp.max) {
      return DigimonConditionConstant.injured;
    }
    return DigimonConditionConstant.healthy;
  }
}
