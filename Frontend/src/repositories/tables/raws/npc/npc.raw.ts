import type { NpcPartyMemberRaw } from "./npc-party-member.raw";
import type { MainQuestAvailabilityWindowRaw } from "@/repositories/tables/raws/quest/main-quest-availability-window.raw";
import type { NpcType } from "@/types/npc-type.type";

export interface NpcRaw {
  type: Exclude<NpcType, "tamer">;
  locationId: string;
  imageName?: string | null;
  mainQuestAvailabilityWindow?: MainQuestAvailabilityWindowRaw;
  party: NpcPartyMemberRaw[];
  exp: number;
  dvexp: number;
  bit: number;
}
