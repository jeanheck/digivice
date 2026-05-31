import type { DigimonStatusType } from "@/models/digimon-status-type";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import type { EquipmentViewModel } from "@/viewmodels/digimon/equipment.viewmodel";

export class EquipmentConverter {
    public static convert(equipmentId: number, equipmentRaw: EquipmentRaw): EquipmentViewModel {
        return {
            id: equipmentId,
            type: equipmentRaw.type,
            attributes: equipmentRaw.attributes.map((equipmentAttributeRaw) => ({
                attribute: equipmentAttributeRaw.attribute as DigimonStatusType,
                type: equipmentAttributeRaw.type,
                value: equipmentAttributeRaw.value,
            })),
            equipableDigimon: equipmentRaw.equipableDigimon,
        };
    }
}
