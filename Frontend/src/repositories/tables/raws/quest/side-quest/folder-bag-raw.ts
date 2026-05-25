import type { RequisiteRaw, StepsRaw } from "../quest-common-raw";

export interface FolderBagRaw {
    Id: string;
    Requisites: RequisiteRaw[];
    Steps: StepsRaw;
}
