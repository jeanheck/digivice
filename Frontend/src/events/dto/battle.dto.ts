import type { EnemyDTO } from "./battles/enemy.dto";

export interface BattleDTO {
  field?: number;
  groupId?: number;
  enemy?: EnemyDTO;
}
