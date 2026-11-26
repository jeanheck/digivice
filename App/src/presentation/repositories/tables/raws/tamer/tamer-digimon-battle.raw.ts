import type { TamerCharismaRequiredRaw } from "./tamer-charisma-required.raw";
import type { TamerDigimonBattlePartyMemberRaw } from "./tamer-digimon-battle-party-member.raw";
import type { TamerTrophyRequiredRaw } from "./tamer-trophy-required.raw";

export interface TamerDigimonBattleRaw {
  charismaRequired: TamerCharismaRequiredRaw;
  party: TamerDigimonBattlePartyMemberRaw[];
  exp: number | null;
  dvexp: number | null;
  bit: number;
  trophyRequired?: TamerTrophyRequiredRaw;
}
