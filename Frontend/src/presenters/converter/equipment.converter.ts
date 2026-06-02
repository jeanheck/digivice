import type { StatKey } from "@/constants/stat-key";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import type { EquipmentViewModel } from "@/viewmodels/digimon/equipment.viewmodel";

export class EquipmentConverter {
    public static convert(equipmentId: number, equipmentRaw: EquipmentRaw): EquipmentViewModel {
        return {
            id: equipmentId,
            type: equipmentRaw.type,
            attributes: equipmentRaw.attributes.map((equipmentAttributeRaw) => ({
                attribute: equipmentAttributeRaw.attribute as StatKey,
                type: equipmentAttributeRaw.type,
                value: equipmentAttributeRaw.value,
            })),
            equipableDigimonIds: equipmentRaw.equipableDigimon.map((digimonId) => Number(digimonId)),
            equipableDigimonNames: [],
        };
    }
}
