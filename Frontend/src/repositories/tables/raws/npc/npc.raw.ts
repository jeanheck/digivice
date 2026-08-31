import type { NpcMainQuestStepDoneRaw } from "./npc-main-quest-step-done.raw";
import type { NpcPartyMemberRaw } from "./npc-party-member.raw";

export interface NpcRaw {
  type: "leader" | "npc";
  locationId: string;
  imageName?: string | null;
  mainQuestStepDone?: NpcMainQuestStepDoneRaw;
  party: NpcPartyMemberRaw[];
  exp: number;
  dvexp: number;
  bit: number;
}
