import { MapId } from "@/shared/constants/map-id.constant";
import type { InBattle } from "@/state/models";

export class DigimonBattleSelector {
  public static isInBattle(mapId: string, inBattle: InBattle): boolean {
    return mapId === MapId.digimonBattle && inBattle.hp.max !== 0;
  }
}
