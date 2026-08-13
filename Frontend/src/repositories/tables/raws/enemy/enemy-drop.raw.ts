export interface EnemyDropRaw {
  id: string;
  locationOnly?: string;
}

export type EnemyDropsByStepRaw = Record<string, EnemyDropRaw[]>;
