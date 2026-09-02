import type { NpcMainQuestStepDoneRaw } from "@/repositories/tables/raws/npc/npc-main-quest-step-done.raw";
import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";

export type EnemyLocationSource = "walking" | "boss" | "fishing" | "kickingTree";

export interface EnemyLocationRaw {
  id: string;
  sources: EnemyLocationSource[];
  localCoordinates?: CoordinatesRaw;
  mainQuestStepDone?: NpcMainQuestStepDoneRaw;
}
