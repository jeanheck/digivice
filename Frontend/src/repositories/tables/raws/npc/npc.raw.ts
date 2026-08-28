import type { NpcTypeConstant } from "@/constants/npc-type.constant";
import type { NpcCardBattleRaw } from "./npc-card-battle.raw";
import type { NpcDigimonBattleRaw } from "./npc-digimon-battle.raw";

export interface NpcRaw {
  name: string;
  type: NpcTypeConstant;
  locationId: string;
  opponentId?: number;
  cardBattles?: Record<string, NpcCardBattleRaw>;
  digimonBattles?: Record<string, NpcDigimonBattleRaw>;
}
