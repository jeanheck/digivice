import type { ResistancesRaw } from "./resistances.raw";
import type { AttributesRaw } from "./attributes.raw";
import type { ElementsRaw } from "./elements.raw";
import type { DigievolutionTechniqueRaw } from "./digievolution-technique.raw";
import type { DigievolutionTier } from "@/types/digievolution-tier.type";

export interface DigievolutionRaw {
  name: string;
  tier: DigievolutionTier;
  attributes: AttributesRaw;
  elements: ElementsRaw;
  resistances: ResistancesRaw;
  techniques: DigievolutionTechniqueRaw[];
}
