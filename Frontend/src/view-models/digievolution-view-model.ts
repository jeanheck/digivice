import type { Attributes } from "@/models/attributes";
import type { Resistances } from "@/models/resistances";

export interface DigievolutionViewModel {
    name: string;
    attributes: Attributes;
    resistances: Resistances;
}