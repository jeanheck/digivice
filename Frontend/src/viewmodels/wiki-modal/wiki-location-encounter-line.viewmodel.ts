import type { EnemyLocationSourceViewModel } from "@/viewmodels/enemy/enemy-location-source.viewmodel";
import type { WikiLocationEncounterEnemyViewModel } from "@/viewmodels/wiki-modal/wiki-location-encounter-enemy.viewmodel";

export interface WikiLocationEncounterLineViewModel {
  source: EnemyLocationSourceViewModel;
  enemies: WikiLocationEncounterEnemyViewModel[];
}
