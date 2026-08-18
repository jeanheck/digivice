import { EquipmentConverter } from "@/presenters/converter/equipment.converter";
import { DigimonRepository } from "@/repositories/digimon.repository";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import type { EquipmentViewModel } from "@/viewmodels/digimon/equipment.viewmodel";

export class WikiDropEquipmentPresenter {
  public static getViewModel(equipmentId: number): EquipmentViewModel {
    const equipmentRaw = EquipmentRepository.getEquipmentById(equipmentId);
    const equipmentViewModel = EquipmentConverter.convert(equipmentId, equipmentRaw);

    return {
      ...equipmentViewModel,
      equipableDigimonNames: equipmentViewModel.equipableDigimonIds.map((digimonId) => {
        return DigimonRepository.getNameById(digimonId);
      }),
    };
  }
}
