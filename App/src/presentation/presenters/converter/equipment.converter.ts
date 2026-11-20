import type { EquipmentRaw } from "@/presentation/repositories/tables/raws/equipment/equipment.raw";
import type { EquipmentViewModel } from "@/presentation/viewmodels/digimon/equipment.viewmodel";

export class EquipmentConverter {
  public static convert(
    equipmentId: number,
    equipableDigimonNames: string[],
    equipmentRaw: EquipmentRaw,
  ): EquipmentViewModel {
    if (equipmentRaw === undefined) {
      return {
        id: equipmentId,
        type: "Non Mapped Equipment",
        attributes: [],
        equipableDigimonIds: [],
        equipableDigimonNames: [],
      };
    }

    return {
      id: equipmentId,
      type: equipmentRaw.type,
      attributes: equipmentRaw.attributes.map((equipmentAttributeRaw) => ({
        attribute: equipmentAttributeRaw.attribute,
        type: equipmentAttributeRaw.type,
        value: equipmentAttributeRaw.value,
      })),
      equipableDigimonIds: equipmentRaw.equipableDigimon.map((digimonId) => Number(digimonId)),
      equipableDigimonNames: equipableDigimonNames,
    };
  }
}
