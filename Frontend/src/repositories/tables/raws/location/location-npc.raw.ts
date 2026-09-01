import type { NpcMainQuestStepDoneRaw } from "@/repositories/tables/raws/npc/npc-main-quest-step-done.raw";

export interface LocationNpcRaw {
  id: string;
  mainQuestStepDone?: NpcMainQuestStepDoneRaw;
}
