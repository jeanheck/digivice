import type { NpcMainQuestStepDoneRaw } from "@/repositories/tables/raws/npc/npc-main-quest-step-done.raw";
import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";
import type { EnemyLocationSourceViewModel } from "@/viewmodels/enemy/enemy-location-source.viewmodel";

export interface EnemyLocationViewModel {
  id: string;
  sources: EnemyLocationSourceViewModel[];
  localCoordinates?: CoordinatesViewModel;
  mainQuestStepDone?: NpcMainQuestStepDoneRaw;
}
