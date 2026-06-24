import type { EquipmentAttributeViewModel } from "@/viewmodels/digimon/equipment-attribute.viewmodel";

export interface EquipmentViewModel {
    id: number;
    type: string;
    attributes: EquipmentAttributeViewModel[];
    equipableDigimonIds: number[];
    equipableDigimonNames: string[];
}
