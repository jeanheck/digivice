import type { RequisiteResource } from "@/memory/resources/journals/quests/requisite.resource";
import type { StepResource } from "@/memory/resources/journals/quests/step.resource";

export interface QuestResource {
  id: string;
  requisites: RequisiteResource[];
  steps: StepResource[];
}
