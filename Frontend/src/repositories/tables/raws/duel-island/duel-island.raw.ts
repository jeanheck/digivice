import type { TamerCardBattleRaw } from "@/repositories/tables/raws/tamer/tamer-card-battle.raw";
import type { TamerDigimonBattleRaw } from "@/repositories/tables/raws/tamer/tamer-digimon-battle.raw";

export interface DuelIslandRaw {
  imageName?: string | null;
  locationId: string;
  opponentId?: number;
  cardBattles?: Record<string, TamerCardBattleRaw>;
  digimonBattles?: Record<string, TamerDigimonBattleRaw>;
}
