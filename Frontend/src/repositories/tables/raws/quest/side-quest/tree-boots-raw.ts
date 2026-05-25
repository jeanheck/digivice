import type { RequisiteRaw, StepsRaw } from "../quest-common-raw";

export interface TreeBootsRaw {
    Id: string;
    Requisites: RequisiteRaw[];
    Steps: StepsRaw;
}
