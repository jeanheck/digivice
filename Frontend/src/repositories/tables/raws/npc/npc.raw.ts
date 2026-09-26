import type { NpcMainQuestStepDoneRaw } from "./npc-main-quest-step-done.raw";
import type { NpcPartyMemberRaw } from "./npc-party-member.raw";
import type { NpcType } from "@/types/npc-type.type";

export interface NpcRaw {
  type: Exclude<NpcType, "tamer">;
  locationId: string;
  imageName?: string | null;
  mainQuestStepDone?: NpcMainQuestStepDoneRaw;
  party: NpcPartyMemberRaw[];
  exp: number;
  dvexp: number;
  bit: number;
}
