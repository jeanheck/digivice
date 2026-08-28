import type { Enemy } from "./battle/enemy";

export interface Battle {
  field: number;
  groupId: number;
  enemy: Enemy;
}
