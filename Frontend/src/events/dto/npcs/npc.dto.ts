import type { NpcBattleDTO } from "./npc-battle.dto";

export interface NpcDTO {
  id: string;
  digimonBattles?: NpcBattleDTO[];
}
