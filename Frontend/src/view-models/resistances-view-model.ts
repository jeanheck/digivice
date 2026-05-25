import type { EnrichedResistance } from "@/models";

export interface ResistancesViewModel {
  fire: EnrichedResistance;
  water: EnrichedResistance;
  ice: EnrichedResistance;
  wind: EnrichedResistance;
  thunder: EnrichedResistance;
  machine: EnrichedResistance;
  dark: EnrichedResistance;
}
