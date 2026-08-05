import { DigimonConditionConstant } from "@/constants/digimon-condition.constant";
import type { Digimon } from "@/models/party/digimon/digimon";
import type { InCombat } from "@/models/party/digimon/in-combat";
import type { Vital } from "@/models/party/digimon/vital";
import { DigimonRepository } from "@/repositories/digimon.repository";

export class ProfilePresenter {
  private static readonly combatLocationId = "0600";

  public static isInCombat(location: string | null, inCombat: InCombat): boolean {
    return location === this.combatLocationId && inCombat.hp.max !== 0;
  }

  public static getHp(digimon: Digimon, isInCombat: boolean): Vital {
    return isInCombat ? digimon.inCombat.hp : digimon.hp;
  }

  public static getMp(digimon: Digimon, isInCombat: boolean): Vital {
    return isInCombat ? digimon.inCombat.mp : digimon.mp;
  }

  public static getCondition(digimon: Digimon, isInCombat: boolean): number {
    return isInCombat ? digimon.inCombat.condition : 0;
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
