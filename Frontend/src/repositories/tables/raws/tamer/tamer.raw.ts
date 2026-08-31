import type { NpcTypeConstant } from "@/constants/npc-type.constant";
import type { TamerCardBattleRaw } from "./tamer-card-battle.raw";
import type { TamerDigimonBattleRaw } from "./tamer-digimon-battle.raw";

export interface TamerRaw {
  imageName?: string | null;
  type: NpcTypeConstant;
  locationId: string;
  opponentId?: number;
  cardBattles?: Record<string, TamerCardBattleRaw>;
  digimonBattles?: Record<string, TamerDigimonBattleRaw>;
}
