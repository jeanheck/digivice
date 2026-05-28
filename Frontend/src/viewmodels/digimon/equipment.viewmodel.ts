import type { EquipmentType } from "@/models/equipment-type";
import type { EquipmentsAttributes } from "@/models/equipments-attributes";

export interface EquipmentViewModel {
    name: Record<string, string>;
    type: EquipmentType;
    attributes: EquipmentsAttributes[];
    equipableDigimon: string[];
}
