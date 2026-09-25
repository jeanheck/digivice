import type { TamerCardBattleRaw } from "./tamer-card-battle.raw";
import type { TamerDigimonBattleRaw } from "./tamer-digimon-battle.raw";

export interface TamerRaw {
  imageName?: string | null;
  locationId: string;
  cardBattles?: Record<string, TamerCardBattleRaw>;
  digimonBattles?: Record<string, TamerDigimonBattleRaw>;
}
