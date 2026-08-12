export type EnemyLocationSource = "enemies" | "fishing" | "kickingTrees";

export interface EnemyLocationRaw {
  locationId: string;
  lastMainQuestStepDone: number;
  source: EnemyLocationSource;
}
