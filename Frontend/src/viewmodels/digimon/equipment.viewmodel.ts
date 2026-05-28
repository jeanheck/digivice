import type { EquipmentType } from "@/models/equipment-type";
import type { EquipmentsAttributes } from "@/models/equipments-attributes";

export interface EquipmentViewModel {
    id: number;
    type: EquipmentType;
    attributes: EquipmentsAttributes[];
    equipableDigimon: string[];
}
