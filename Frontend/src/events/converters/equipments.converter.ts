import type { EquipmentsDTO } from '../events.map';
import type { Equipments } from '../../models';
import { EquipamentConverter } from './equipament.converter';

export class EquipmentsConverter {
    public static convert(equipmentsDto: EquipmentsDTO | null): Equipments {
        return {
            head: EquipamentConverter.convert(equipmentsDto?.head ?? 0),
            body: EquipamentConverter.convert(equipmentsDto?.body ?? 0),
            rightHand: EquipamentConverter.convert(equipmentsDto?.rightHand ?? 0),
            leftHand: EquipamentConverter.convert(equipmentsDto?.leftHand ?? 0),
            accessory1: EquipamentConverter.convert(equipmentsDto?.accessory1 ?? 0),
            accessory2: EquipamentConverter.convert(equipmentsDto?.accessory2 ?? 0)
        };
    }
}
