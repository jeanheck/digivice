import { DigimonDebuff } from "@/constants/digimon-debuff.constant";
import type { Vital } from "@/models";
import type { DigimonStatus } from "@/types/digimon-status.type";

export class DigimonService {
  public static getStatus(condition: number, hp: Vital): DigimonStatus {
    if (hp.current === 0) {
      return "ko";
    }

    if (condition !== 0) {
      return "debuffed";
    }

    if (hp.current < hp.max) {
      return "injured";
    }

    return "healthy";
  }

  public static getActiveDebuffs(condition: number): DigimonDebuff[] {
    return (Object.keys(DigimonDebuff) as DigimonDebuff[]).filter((debuff) => {
      return (condition & DigimonDebuff[debuff]) !== 0;
    });
  }
}
