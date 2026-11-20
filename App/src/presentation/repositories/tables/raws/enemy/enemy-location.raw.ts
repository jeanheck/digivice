import type { CoordinatesRaw } from "@/presentation/repositories/tables/raws/quest/coordinates.raw";
import type { MainQuestAvailabilityWindowRaw } from "@/presentation/repositories/tables/raws/quest/main-quest-availability-window.raw";
import type { EnemySource } from "@/shared/types/enemy-source.type";

export interface EnemyLocationRaw {
  id: string;
  sources: EnemySource[];
  localCoordinates?: CoordinatesRaw;
  mainQuestAvailabilityWindow?: MainQuestAvailabilityWindowRaw;
}
