import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";
import type { MainQuestStepDoneViewModel } from "@/viewmodels/quest/main-quest-step-done.viewmodel";
import type { EnemySource } from "@/types/enemy-source.type";

export interface EnemyLocationViewModel {
  id: string;
  labelKey: string;
  sources: EnemySource[];
  localCoordinates?: CoordinatesViewModel;
  mainQuestStepDone?: MainQuestStepDoneViewModel;
}
