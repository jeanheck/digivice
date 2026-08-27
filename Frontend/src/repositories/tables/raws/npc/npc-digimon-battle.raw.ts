import type { NpcCharismaRequiredRaw } from "./npc-charisma-required.raw";
import type { NpcTrophyRequiredRaw } from "./npc-trophy-required.raw";

export interface NpcDigimonBattleRaw {
  charismaRequired: NpcCharismaRequiredRaw;
  npcPartyId: string;
  exp: number;
  dvexp: number;
  bit: number;
  trophyRequired?: NpcTrophyRequiredRaw;
}
