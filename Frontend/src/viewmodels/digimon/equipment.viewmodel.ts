import type { EquipmentsAttributes } from "@/models/equipments-attributes";
import type { EquipmentTypeViewModel } from "@/viewmodels/digimon/equipment-type.viewmodel";

export interface EquipmentViewModel {
    id: number;
    type: EquipmentTypeViewModel;
    attributes: EquipmentsAttributes[];
    equipableDigimon: string[];
}
