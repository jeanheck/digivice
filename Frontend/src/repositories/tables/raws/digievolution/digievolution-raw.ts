import type { ResistancesRaw } from "./resistances-raw";
import type { AttributesRaw } from "./attributes-raw";

export interface DigievolutionRaw {
    name: string;
    attributes: AttributesRaw;
    resistances: ResistancesRaw;
}