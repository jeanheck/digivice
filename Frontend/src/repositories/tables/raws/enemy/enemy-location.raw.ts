import type { NpcMainQuestStepDoneRaw } from "@/repositories/tables/raws/npc/npc-main-quest-step-done.raw";
import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";
import type { EnemySource } from "@/types/enemy-source.type";

export interface EnemyLocationRaw {
  id: string;
  sources: EnemySource[];
  localCoordinates?: CoordinatesRaw;
  mainQuestStepDone?: NpcMainQuestStepDoneRaw;
}
