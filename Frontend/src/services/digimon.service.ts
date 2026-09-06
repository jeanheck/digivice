import { DigimonStatusConstant } from "@/constants/digimon-status.constant";
import type { Vital } from "@/models/party/digimon/vital";

export class DigimonService {
  public static getStatus(condition: number, hp: Vital): DigimonStatusConstant {
    if (hp.current === 0) {
      return DigimonStatusConstant.knockedOut;
    }

    if (condition !== 0) {
      return DigimonStatusConstant.debuffed;
    }

    if (hp.current < hp.max) {
      return DigimonStatusConstant.injured;
    }

    return DigimonStatusConstant.healthy;
  }
}
