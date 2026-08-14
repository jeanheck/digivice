import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";

export type EnemyLocationSource = "walking" | "boss" | "fishing" | "kickingTree";

export interface EnemyLocationRaw {
  id: string;
  sources: EnemyLocationSource[];
  localCoordinates?: CoordinatesRaw;
  startWhenLastMainQuestStepDone?: string;
  finishWhenLastMainQuestStepDone?: string;
  accessibleWhen?: string;
}
