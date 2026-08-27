import type { NpcBattle } from "./npc-battle";

export interface Npc {
  id: string;
  digimonBattles: NpcBattle[];
}
