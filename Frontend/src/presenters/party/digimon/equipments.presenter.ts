import { EquipmentConstant } from "@/constants/equipment.constant";
import type { Equipments } from "@/models";
import { EquipmentConverter } from "@/presenters/converter/equipment.converter";
import { DigimonRepository } from "@/repositories/digimon.repository";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import type { EquipmentViewModel } from "@/viewmodels/digimon/equipment.viewmodel";

export class EquipmentsPresenter {
    public static getEquipmentBySlot(equipments: Equipments, slotKey: EquipmentConstant): EquipmentViewModel | null {
        const equipmentId = equipments[slotKey];

        if (!equipmentId) {
            return null;
        }

        const equipmentRaw = EquipmentRepository.getEquipmentById(equipmentId);
        const equipmentViewModel = EquipmentConverter.convert(equipmentId, equipmentRaw);

        return this.enrichEquipableDigimonNames(equipmentViewModel);
    }

    private static enrichEquipableDigimonNames(equipmentViewModel: EquipmentViewModel): EquipmentViewModel {
        return {
            ...equipmentViewModel,
            equipableDigimonNames: equipmentViewModel.equipableDigimonIds.map((digimonId) => {
                return DigimonRepository.getNameById(digimonId);
            }),
        };
    }
}
