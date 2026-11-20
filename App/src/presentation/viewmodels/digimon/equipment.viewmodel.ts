import type { EquipmentAttributeViewModel } from "@/presentation/viewmodels/digimon/equipment-attribute.viewmodel";

export interface EquipmentViewModel {
  id: number;
  type: string;
  attributes: EquipmentAttributeViewModel[];
  equipableDigimonIds: number[];
  equipableDigimonNames: string[];
}
