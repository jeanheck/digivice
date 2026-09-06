import { DigimonConditionConstant } from "@/constants/digimon-condition.constant";
import type { Vital } from "@/models/party/digimon/vital";

export class DigimonService {
  public static getStatus(condition: number, hp: Vital): DigimonConditionConstant {
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
