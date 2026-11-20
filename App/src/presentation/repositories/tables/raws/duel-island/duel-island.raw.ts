import type { TamerCardBattleRaw } from "@/presentation/repositories/tables/raws/tamer/tamer-card-battle.raw";
import type { TamerDigimonBattleRaw } from "@/presentation/repositories/tables/raws/tamer/tamer-digimon-battle.raw";

export interface DuelIslandRaw {
  imageName?: string | null;
  locationId: string;
  cardBattles?: Record<string, TamerCardBattleRaw>;
  digimonBattles?: Record<string, TamerDigimonBattleRaw>;
}
