import type { RequisiteRaw as RequisiteRaw, StepsRaw as StepsRaw } from "./quest-common-raw";

export interface MainQuestRaw {
    Id: string;
    Requisites: RequisiteRaw[];
    Steps: StepsRaw;
}
