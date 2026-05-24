import type { EquipmentType } from '../equipment-type';
import type { EquipmentsAttributes } from '../equipments-attributes';

export interface EnrichedEquipment {
    name: Record<string, string>;
    type: EquipmentType;
    attributes: EquipmentsAttributes[];
    equipableDigimon: string[];
}
