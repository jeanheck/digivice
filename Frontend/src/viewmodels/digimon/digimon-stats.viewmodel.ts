import type { AttributesViewModel } from "@/viewmodels/digimon/attributes.viewmodel";
import type { ElementsViewModel } from "@/viewmodels/digimon/elements.viewmodel";

export interface DigimonStatsViewModel {
  attributes: AttributesViewModel;
  elements: ElementsViewModel;
}
