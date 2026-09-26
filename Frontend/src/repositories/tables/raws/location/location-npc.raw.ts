import type { NpcMainQuestStepDoneRaw } from "@/repositories/tables/raws/npc/npc-main-quest-step-done.raw";
import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";
import type { LabelPlacement } from "@/types/label-placement.type";

export interface LocationNpcRaw {
  id: string;
  coordinates?: CoordinatesRaw;
  labelPlacement?: LabelPlacement;
  mainQuestStepDone?: NpcMainQuestStepDoneRaw;
}
