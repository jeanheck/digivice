import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";
import type { MainQuestAvailabilityWindowViewModel } from "@/viewmodels/quest/main-quest-availability-window.viewmodel";
import type { EnemySource } from "@/types/enemy-source.type";

export interface EnemyLocationViewModel {
  id: string;
  labelKey: string;
  sources: EnemySource[];
  localCoordinates?: CoordinatesViewModel;
  mainQuestAvailabilityWindow?: MainQuestAvailabilityWindowViewModel;
}
