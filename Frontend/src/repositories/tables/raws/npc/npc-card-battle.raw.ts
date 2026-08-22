import type { NpcCharismaRequiredRaw } from "./npc-charisma-required.raw";

export interface NpcCardBattleRaw {
  charismaRequired: NpcCharismaRequiredRaw;
  deckId: string;
  dropId: string;
  asukaTrophyRequired?: boolean;
}
