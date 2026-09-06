import { resolveStatusAilment } from "@/constants/digimon-status-ailment.constant";
import { DigimonStatusConstant } from "@/constants/digimon-status.constant";
import type { Digimon } from "@/models/party/digimon/digimon";
import type { InBattle } from "@/models/party/digimon/in-battle";
import type { Vital } from "@/models/party/digimon/vital";
import { DigimonRepository } from "@/repositories/digimon.repository";
import { DigimonBattleService } from "@/services/digimon-battle.service";
import { DigimonService } from "@/services/digimon.service";

export class ProfilePresenter {
  public static isInBattle(location: string | null, inBattle: InBattle): boolean {
    return DigimonBattleService.isInBattle(location, inBattle);
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

  public static getConditionTooltipKey(condition: number, hp: Vital): string {
    const status = DigimonService.getStatus(condition, hp);

    if (status === DigimonStatusConstant.knockedOut) {
      return "digimon.status.knockedOut";
    }
    if (status === DigimonStatusConstant.injured) {
      return "digimon.status.injured";
    }
    if (status === DigimonStatusConstant.healthy) {
      return "digimon.status.healthy";
    }

    const statusAilment = resolveStatusAilment(condition);
    if (statusAilment !== null) {
      return `conditions.${statusAilment}.affected`;
    }

    return "digimon.condition";
  }
}
