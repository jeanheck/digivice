export type EnemyLocationSource = "walking" | "fishing" | "kickingTree";

export interface EnemyLocationRaw {
  locationId: string;
  lastMainQuestStepDone: number;
  source: EnemyLocationSource;
}
