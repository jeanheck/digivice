import type { EquipmentsAttributes } from "@/models/equipments-attributes";

export interface EquipmentViewModel {
    id: number;
    type: string;
    attributes: EquipmentsAttributes[];
    equipableDigimon: string[];
}
