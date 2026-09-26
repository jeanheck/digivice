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
        slotKey: "head",
        equipment: this.getEquipmentViewModel(equipments.head),
      },
      {
        slotKey: "body",
        equipment: this.getEquipmentViewModel(equipments.body),
      },
      {
        slotKey: "right",
        equipment: this.getEquipmentViewModel(equipments.right),
      },
      {
        slotKey: "left",
        equipment: this.getEquipmentViewModel(equipments.left),
      },
      {
        slotKey: "accessory1",
        equipment: this.getEquipmentViewModel(equipments.accessory1),
      },
      {
        slotKey: "accessory2",
        equipment: this.getEquipmentViewModel(equipments.accessory2),
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
