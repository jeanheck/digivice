import type { RequisiteResource } from "@/memory/resources/journals/quests/requisite.resource";

export interface StepResource {
  number: number;
  value: number;
  requisites: RequisiteResource[];
}
