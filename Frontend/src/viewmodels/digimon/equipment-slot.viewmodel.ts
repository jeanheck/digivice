import type { EquipmentConstant } from "@/constants/equipment.constant";
import type { EquipmentViewModel } from "@/viewmodels/digimon/equipment.viewmodel";

export interface EquipmentSlotViewModel {
  slotKey: EquipmentConstant;
  equipment: EquipmentViewModel | null;
}
