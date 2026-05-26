import type { RequisiteViewModel } from "./requisite-view-model";
import type { StepViewModel } from "./step-view-model";

export interface QuestViewModel {
    id: string;
    requisites: RequisiteViewModel[];
    steps: StepViewModel[];
}