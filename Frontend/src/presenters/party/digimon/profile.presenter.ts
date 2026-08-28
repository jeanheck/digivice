import { DigimonConditionConstant } from "@/constants/digimon-condition.constant";
import { ConditionConstant } from "@/constants/stat/condition.constant";
import type { Digimon } from "@/models/party/digimon/digimon";
import type { InBattle } from "@/models/party/digimon/in-battle";
import type { Vital } from "@/models/party/digimon/vital";
import { DigimonRepository } from "@/repositories/digimon.repository";

export class ProfilePresenter {
  private static readonly battleLocationId = "0600";

  private static readonly conditionBitByStatus: ReadonlyArray<{
    bitMask: number;
    status: ConditionConstant;
  }> = [
    { bitMask: 0x01, status: ConditionConstant.poison },
    { bitMask: 0x02, status: ConditionConstant.paralyze },
    { bitMask: 0x04, status: ConditionConstant.confuse },
    { bitMask: 0x08, status: ConditionConstant.sleep },
  ];

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

  public static getConditionTooltipKey(condition: number, hp: Vital): string {
    const calculatedCondition = this.getCalculatedCondition(condition, hp);

    if (calculatedCondition === DigimonConditionConstant.ko) {
      return "digimon.conditionState.ko";
    }
    if (calculatedCondition === DigimonConditionConstant.injured) {
      return "digimon.conditionState.injured";
    }
    if (calculatedCondition === DigimonConditionConstant.healthy) {
      return "digimon.conditionState.healthy";
    }

    for (const entry of this.conditionBitByStatus) {
      if ((condition & entry.bitMask) !== 0) {
        return `conditions.${entry.status}.affected`;
      }
    }

    return "digimon.condition";
  }
}
