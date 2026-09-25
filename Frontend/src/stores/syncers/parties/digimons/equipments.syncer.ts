import type { Equipments } from "@/models";
import type { EquipmentsDTO } from "@/events/dto/parties/digimons/equipments.dto";
import { EquipmentsConverter } from "@/events/converters/parties/digimons/equipments.converter";

export class EquipmentsSyncer {
  public static sync(previousEquipments: Equipments, newEquipmentsDto: EquipmentsDTO): void {
    if (newEquipmentsDto.head !== undefined) {
      previousEquipments.head = EquipmentsConverter.toEquipmentId(newEquipmentsDto.head);
    }
    if (newEquipmentsDto.body !== undefined) {
      previousEquipments.body = EquipmentsConverter.toEquipmentId(newEquipmentsDto.body);
    }
    if (newEquipmentsDto.right !== undefined) {
      previousEquipments.right = EquipmentsConverter.toEquipmentId(newEquipmentsDto.right);
    }
    if (newEquipmentsDto.left !== undefined) {
      previousEquipments.left = EquipmentsConverter.toEquipmentId(newEquipmentsDto.left);
    }
    if (newEquipmentsDto.accessory1 !== undefined) {
      previousEquipments.accessory1 = EquipmentsConverter.toEquipmentId(newEquipmentsDto.accessory1);
    }
    if (newEquipmentsDto.accessory2 !== undefined) {
      previousEquipments.accessory2 = EquipmentsConverter.toEquipmentId(newEquipmentsDto.accessory2);
    }
  }
}
