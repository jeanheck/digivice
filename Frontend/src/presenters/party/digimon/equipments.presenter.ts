import { EquipmentConstant } from "@/constants/equipment.constant";
import type { Equipments } from "@/models";
import { EquipmentConverter } from "@/presenters/converter/equipment.converter";
import { DigimonRepository } from "@/repositories/digimon.repository";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import type { EquipmentSlotViewModel } from "@/viewmodels/digimon/equipment-slot.viewmodel";
import type { EquipmentViewModel } from "@/viewmodels/digimon/equipment.viewmodel";

export class EquipmentsPresenter {
  public static getEquipmentsViewModel(equipments: Equipments): EquipmentSlotViewModel[] {
    return [
      {
        slotKey: EquipmentConstant.head,
        equipment: this.getEquipmentViewModel(equipments[EquipmentConstant.head]),
      },
      {
        slotKey: EquipmentConstant.body,
        equipment: this.getEquipmentViewModel(equipments[EquipmentConstant.body]),
      },
      {
        slotKey: EquipmentConstant.right,
        equipment: this.getEquipmentViewModel(equipments[EquipmentConstant.right]),
      },
      {
        slotKey: EquipmentConstant.left,
        equipment: this.getEquipmentViewModel(equipments[EquipmentConstant.left]),
      },
      {
        slotKey: EquipmentConstant.accessory1,
        equipment: this.getEquipmentViewModel(equipments[EquipmentConstant.accessory1]),
      },
      {
        slotKey: EquipmentConstant.accessory2,
        equipment: this.getEquipmentViewModel(equipments[EquipmentConstant.accessory2]),
      },
    ];
  }

  private static getEquipmentViewModel(equipmentId: number | null): EquipmentViewModel | null {
    if (!equipmentId) {
      return null;
    }

    const equipmentRaw = EquipmentRepository.getEquipmentById(equipmentId);
    const equipableDigimonNames = equipmentRaw.equipableDigimon.map((digimonId) => {
      return DigimonRepository.getNameById(Number(digimonId));
    });
    return EquipmentConverter.convert(equipmentId, equipableDigimonNames, equipmentRaw);
  }
}
