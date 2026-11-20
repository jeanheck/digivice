import type { AttributesViewModel } from "@/presentation/viewmodels/digimon/attributes.viewmodel";
import type { ElementsViewModel } from "@/presentation/viewmodels/digimon/elements.viewmodel";

export interface DigimonStatsViewModel {
  attributes: AttributesViewModel;
  elements: ElementsViewModel;
}
