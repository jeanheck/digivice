import type { ResistancesRaw } from "./resistances.raw";
import type { AttributesRaw } from "./attributes.raw";
import type { ElementsRaw } from "./elements.raw";

export interface DigievolutionRaw {
  name: string;
  attributes: AttributesRaw;
  elements: ElementsRaw;
  resistances: ResistancesRaw;
}
