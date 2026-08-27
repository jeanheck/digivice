import type { NpcCharismaRequiredRaw } from "./npc-charisma-required.raw";
import type { NpcTrophyRequiredRaw } from "./npc-trophy-required.raw";

export interface NpcCardBattleRaw {
  charismaRequired: NpcCharismaRequiredRaw;
  deckId: string;
  dropId: string;
  trophyRequired?: NpcTrophyRequiredRaw;
}
