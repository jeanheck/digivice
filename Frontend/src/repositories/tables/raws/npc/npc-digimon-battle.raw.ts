import type { NpcCharismaRequiredRaw } from "./npc-charisma-required.raw";

export interface NpcDigimonBattleRaw {
  charismaRequired: NpcCharismaRequiredRaw;
  npcPartyId: string;
  exp: number;
  dvexp: number;
  bit: number;
  asukaTrophyRequired?: boolean;
}
