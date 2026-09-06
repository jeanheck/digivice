import { DigimonStatusConstant } from "@/constants/digimon-status.constant";
import type { Vital } from "@/models/party/digimon/vital";
import { DigimonService } from "@/services/digimon.service";

export class DigimonBattleEnemyStatusPresenter {
  public static getStatus(condition: number, hp: Vital): DigimonStatusConstant {
    return DigimonService.getStatus(condition, hp);
  }
}
