import { MapId } from "@/constants/map-id.constant";
import type { InBattle } from "@/models";

export class DigimonBattleSelector {
  public static isInBattle(mapId: string, inBattle: InBattle): boolean {
    return mapId === MapId.digimonBattle && inBattle.hp.max !== 0;
  }
}
