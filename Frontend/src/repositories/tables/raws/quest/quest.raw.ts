import type { RequisiteRaw } from "./requisite.raw";
import type { StepsRaw } from "./steps.raw";

export interface QuestRaw {
    id: string;
    requisites: RequisiteRaw[];
    steps: StepsRaw;
}
