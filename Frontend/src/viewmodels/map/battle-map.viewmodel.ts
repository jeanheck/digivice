import type { Vital } from "@/models/party/digimon/vital";
import type { EnemyConditionViewModel } from "@/viewmodels/enemy/enemy-condition.viewmodel";
import type { EnemyStatViewModel } from "@/viewmodels/enemy/enemy-stat.viewmodel";

export interface BattleMapViewModel {
  enemyId: string | null;
  title: string;
  isBoss: boolean;
  level: number | null;
  species: string | null;
  speciesEmoji: string | null;
  hp: Vital;
  attributes: EnemyStatViewModel[];
  elements: EnemyStatViewModel[];
  conditions: EnemyConditionViewModel[];
  enemyImageUrl: string | null;
}
