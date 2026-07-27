import type { RequisiteViewModel } from "./requisite.viewmodel";
import type { StepViewModel } from "./step.viewmodel";

export type QuestCardVariant = "locked" | "unavailable" | "done" | "new" | "active";

export interface QuestViewModel {
    id: string;
    requisites: RequisiteViewModel[];
    steps: StepViewModel[];
    isDone: boolean;
    isLocked: boolean;
    isUnavailable: boolean;
    isNew: boolean;
    currentStep: StepViewModel | null;
    cardVariant: QuestCardVariant;
}
