import type { RequisiteRaw as RequisiteRaw, StepsRaw as StepsRaw } from "./quest-common-raw";

export interface QuestRaw {
    id: string;
    requisites: RequisiteRaw[];
    steps: StepsRaw;
}
