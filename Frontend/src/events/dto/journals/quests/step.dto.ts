import type { RequisiteDTO } from "@/events/dto/journals/quests/requisite.dto";

export interface StepDTO {
  number: number;
  isDone?: boolean;
  requisites?: RequisiteDTO[];
}
