import type { EquipmentsDTO } from "@/events/dto/parties/digimons/equipments.dto";
import type { Equipments } from "@/models";

export class EquipmentsConverter {
  public static toEquipmentId(value: number | undefined): number | null {
    if (value === undefined || value === 0) {
      return null;
    }

    return value;
  }

  public static convert(equipmentsDto: EquipmentsDTO | null): Equipments {
    return {
      head: EquipmentsConverter.toEquipmentId(equipmentsDto?.head),
      body: EquipmentsConverter.toEquipmentId(equipmentsDto?.body),
      right: EquipmentsConverter.toEquipmentId(equipmentsDto?.right),
      left: EquipmentsConverter.toEquipmentId(equipmentsDto?.left),
      accessory1: EquipmentsConverter.toEquipmentId(equipmentsDto?.accessory1),
      accessory2: EquipmentsConverter.toEquipmentId(equipmentsDto?.accessory2),
    };
  }
}
