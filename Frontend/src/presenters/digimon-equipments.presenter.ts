import { EquipmentSlotKey } from "@/constants/equipment-slot-key";
import type { Equipments } from "@/models";
import type { DigimonStatusType } from "@/models/digimon-status-type";
import type { EquipmentsAttributesOperationType } from "@/models/equipments-attributes-operation-type";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import type { EquipmentViewModel } from "@/viewmodels/digimon/equipment.viewmodel";

export class DigimonEquipmentsPresenter {
    public static getEquipmentBySlot(equipments: Equipments, slotKey: EquipmentSlotKey): EquipmentViewModel | null {
        const equipmentId = equipments[slotKey];

        if (!equipmentId) {
            return null;
        }

        const equipmentRaw = EquipmentRepository.getRawEquipmentById(equipmentId);

        return {
            name: equipmentRaw.name,
            type: equipmentRaw.type as EquipmentViewModel["type"],
            attributes: equipmentRaw.attributes.map((equipmentAttributeRaw) => ({
                attribute: equipmentAttributeRaw.attribute as DigimonStatusType,
                type: equipmentAttributeRaw.type as EquipmentsAttributesOperationType,
                value: equipmentAttributeRaw.value,
            })),
            equipableDigimon: equipmentRaw.equipableDigimon,
        };
    }
}
