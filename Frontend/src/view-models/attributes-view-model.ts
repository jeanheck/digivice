import type { EnrichedAttribute } from "@/models";

export interface AttributesViewModel {
  strength: EnrichedAttribute;
  defense: EnrichedAttribute;
  spirit: EnrichedAttribute;
  wisdom: EnrichedAttribute;
  speed: EnrichedAttribute;
  charisma: EnrichedAttribute;
}
