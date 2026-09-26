import type { EnemySource } from "@/types/enemy-source.type";
import type { WikiLocationEncounterEnemyViewModel } from "@/viewmodels/wiki-modal/wiki-location-encounter-enemy.viewmodel";

export interface WikiLocationEncounterLineViewModel {
  source: EnemySource;
  enemies: WikiLocationEncounterEnemyViewModel[];
}
