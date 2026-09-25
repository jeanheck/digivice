import type { DeepRequired } from "@/events/dto/deep-required";
import type { EquipmentsDTO } from "@/events/dto/parties/digimons/equipments.dto";
import type { Equipments } from "@/models";

export class EquipmentsConverter {
  public static convert(equipmentsDto: DeepRequired<EquipmentsDTO>): Equipments {
    return {
      head: equipmentsDto.head,
      body: equipmentsDto.body,
      right: equipmentsDto.right,
      left: equipmentsDto.left,
      accessory1: equipmentsDto.accessory1,
      accessory2: equipmentsDto.accessory2,
    };
  }
}
