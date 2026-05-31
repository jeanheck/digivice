import { EquipmentSlotKey } from "@/constants/equipment-slot-key";
import type { Equipments } from "@/models";
import { EquipmentConverter } from "@/presenters/converter/equipment.converter";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import type { EquipmentViewModel } from "@/viewmodels/digimon/equipment.viewmodel";

export class DigimonEquipmentsPresenter {
    public static getEquipmentBySlot(equipments: Equipments, slotKey: EquipmentSlotKey): EquipmentViewModel | null {
        const equipmentId = equipments[slotKey];

        if (!equipmentId) {
            return null;
        }

        const equipmentRaw = EquipmentRepository.getEquipmentById(equipmentId);

        return EquipmentConverter.convert(equipmentId, equipmentRaw);
    }
}
