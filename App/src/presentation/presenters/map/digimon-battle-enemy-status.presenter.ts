import type { Vital } from "@/state/models";
import { DigimonService } from "@/presentation/services/digimon.service";
import type { DigimonStatus } from "@/shared/types/digimon-status.type";

export class DigimonBattleEnemyStatusPresenter {
  public static getStatus(condition: number, hp: Vital): DigimonStatus {
    return DigimonService.getStatus(condition, hp);
  }
}
