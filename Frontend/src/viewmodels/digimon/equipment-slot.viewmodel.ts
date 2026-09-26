import type { EquipmentSlot } from "@/types/equipment-slot.type";
import type { EquipmentViewModel } from "@/viewmodels/digimon/equipment.viewmodel";

export interface EquipmentSlotViewModel {
  slotKey: EquipmentSlot;
  equipment: EquipmentViewModel | null;
}
