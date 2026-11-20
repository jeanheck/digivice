import { EquipmentConverter } from "@/presentation/presenters/converter/equipment.converter";
import { DigimonRepository } from "@/presentation/repositories/digimon.repository";
import { EquipmentRepository } from "@/presentation/repositories/equipment.repository";
import type { EquipmentViewModel } from "@/presentation/viewmodels/digimon/equipment.viewmodel";

export class WikiDropEquipmentPresenter {
  public static getViewModel(equipmentId: number): EquipmentViewModel {
    const equipmentRaw = EquipmentRepository.getEquipmentById(equipmentId);
    const equipableDigimonNames = equipmentRaw.equipableDigimon.map((digimonId) => {
      return DigimonRepository.getNameById(Number(digimonId));
    });
    return EquipmentConverter.convert(equipmentId, equipableDigimonNames, equipmentRaw);
  }
}
