import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";
import type { EnemyLocationSourceViewModel } from "@/viewmodels/enemy/enemy-location-source.viewmodel";
import type { MainQuestStepDoneViewModel } from "@/viewmodels/quest/main-quest-step-done.viewmodel";

export interface EnemyLocationViewModel {
  id: string;
  sources: EnemyLocationSourceViewModel[];
  localCoordinates?: CoordinatesViewModel;
  mainQuestStepDone?: MainQuestStepDoneViewModel;
}
