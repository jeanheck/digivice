export type EnemyLocationSource = "walking" | "fishing" | "kickingTree";

export interface EnemyLocationEntryRaw {
  sources: EnemyLocationSource[];
}

export type EnemyLocationsByStepRaw = Record<string, Record<string, EnemyLocationEntryRaw>>;
