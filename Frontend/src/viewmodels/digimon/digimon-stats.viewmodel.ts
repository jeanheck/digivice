import type { AttributesViewModel } from "@/viewmodels/digimon/attributes.viewmodel";
import type { ResistancesViewModel } from "@/viewmodels/digimon/resistances.viewmodel";

export interface DigimonStatsViewModel {
    attributes: AttributesViewModel;
    resistances: ResistancesViewModel;
}
