import type { Equipments } from '@/models';
import type { EquipmentsDTO } from '@/events/dto/parties/digimons/equipments.dto';

export class EquipmentsSyncer {
    public static sync(previousEquipments: Equipments, newEquipmentsDto: EquipmentsDTO): void {
        if (newEquipmentsDto.head !== undefined) {
            previousEquipments.head = newEquipmentsDto.head;
        }
        if (newEquipmentsDto.body !== undefined) {
            previousEquipments.body = newEquipmentsDto.body;
        }
        if (newEquipmentsDto.rightHand !== undefined) {
            previousEquipments.rightHand = newEquipmentsDto.rightHand;
        }
        if (newEquipmentsDto.leftHand !== undefined) {
            previousEquipments.leftHand = newEquipmentsDto.leftHand;
        }
        if (newEquipmentsDto.accessory1 !== undefined) {
            previousEquipments.accessory1 = newEquipmentsDto.accessory1;
        }
        if (newEquipmentsDto.accessory2 !== undefined) {
            previousEquipments.accessory2 = newEquipmentsDto.accessory2;
        }
    }
}
