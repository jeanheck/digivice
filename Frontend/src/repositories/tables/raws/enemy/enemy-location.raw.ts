export type EnemyLocationSource = "walking" | "boss" | "fishing" | "kickingTree";

export interface EnemyLocationRaw {
  id: string;
  sources: EnemyLocationSource[];
  startWhenLastMainQuestStepDone?: string;
  finishWhenLastMainQuestStepDone?: string;
  accessibleWhen?: string;
}
