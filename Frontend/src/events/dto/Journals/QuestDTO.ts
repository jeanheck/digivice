import type { RequisiteDTO } from './Quests/RequisiteDTO';
import type { StepDTO } from './Quests/StepDTO';

export interface QuestDTO {
    id: string;
    requisites?: RequisiteDTO[];
    steps?: StepDTO[];
}
