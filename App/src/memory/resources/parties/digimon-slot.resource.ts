import type { DigimonResource } from "@/memory/resources/parties/digimon.resource";

export interface DigimonSlotResource {
  index: number;
  digimonId: number;
  digimon: DigimonResource | null;
}
