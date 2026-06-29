import type { RequisiteDTO } from './quests/requisite.dto';
import type { StepDTO } from './quests/step.dto';

export interface QuestDTO {
    id: string;
    requisites?: RequisiteDTO[];
    steps?: StepDTO[];
}
