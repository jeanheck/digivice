import type { RequisiteViewModel } from "./requisite.viewmodel";
import type { StepViewModel } from "./step.viewmodel";

export interface QuestViewModel {
    id: string;
    requisites: RequisiteViewModel[];
    steps: StepViewModel[];
}