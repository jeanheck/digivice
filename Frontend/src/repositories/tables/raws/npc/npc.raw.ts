import type { NpcCardBattleRaw } from "./npc-card-battle.raw";
import type { NpcDigimonBattleRaw } from "./npc-digimon-battle.raw";

export interface NpcRaw {
  name: string;
  locationId: string;
  cardBattles?: NpcCardBattleRaw[];
  digimonBattles?: NpcDigimonBattleRaw[];
}
