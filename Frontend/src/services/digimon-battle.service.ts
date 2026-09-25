import type { InBattle } from "@/models/party/digimon/in-battle";

const BATTLE_LOCATION_ID = "0600";

export class DigimonBattleService {
  public static isInBattle(location: string | null, inBattle: InBattle): boolean {
    return location === BATTLE_LOCATION_ID && inBattle.hp.max !== 0;
  }
}
