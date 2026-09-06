import { DigimonConditionConstant } from "@/constants/digimon-condition.constant";
import type { Vital } from "@/models/party/digimon/vital";
import { DigimonService } from "@/services/digimon.service";

export class DigimonBattleEnemyStatusPresenter {
  public static getStatus(condition: number, hp: Vital): DigimonConditionConstant {
    return DigimonService.getStatus(condition, hp);
  }
}
