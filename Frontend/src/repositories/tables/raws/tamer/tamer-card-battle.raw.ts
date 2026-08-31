import type { TamerCharismaRequiredRaw } from "./tamer-charisma-required.raw";
import type { TamerTrophyRequiredRaw } from "./tamer-trophy-required.raw";

export interface TamerCardBattleRaw {
  charismaRequired: TamerCharismaRequiredRaw;
  deckId: string;
  dropId: string;
  trophyRequired?: TamerTrophyRequiredRaw;
}
