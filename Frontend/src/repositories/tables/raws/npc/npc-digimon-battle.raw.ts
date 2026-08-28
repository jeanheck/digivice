import type { NpcCharismaRequiredRaw } from "./npc-charisma-required.raw";
import type { NpcDigimonBattlePartyMemberRaw } from "./npc-digimon-battle-party-member.raw";
import type { NpcTrophyRequiredRaw } from "./npc-trophy-required.raw";

export interface NpcDigimonBattleRaw {
  charismaRequired: NpcCharismaRequiredRaw;
  party: NpcDigimonBattlePartyMemberRaw[];
  exp: number;
  dvexp: number;
  bit: number;
  trophyRequired?: NpcTrophyRequiredRaw;
}
