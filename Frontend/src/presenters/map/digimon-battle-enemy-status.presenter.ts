import type { Vital } from "@/models";
import { DigimonService } from "@/services/digimon.service";
import type { DigimonStatus } from "@/types/digimon-status.type";

export class DigimonBattleEnemyStatusPresenter {
  public static getStatus(condition: number, hp: Vital): DigimonStatus {
    return DigimonService.getStatus(condition, hp);
  }
}
