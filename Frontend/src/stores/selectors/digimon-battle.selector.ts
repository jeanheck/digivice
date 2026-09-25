import { MapIdConstant } from "@/constants/map-id.constant";
import type { InBattle } from "@/models";

export class DigimonBattleSelector {
  public static isInBattle(mapId: string | null, inBattle: InBattle): boolean {
    return mapId === MapIdConstant.digimonBattle && inBattle.hp.max !== 0;
  }
}
