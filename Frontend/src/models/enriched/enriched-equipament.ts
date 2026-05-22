import type { EquipmentType } from '../equipment-type';
import type { EquipmentsAttributes } from '../equipments-attributes';

export interface EnrichedEquipment {
    id: number;
    name: Record<string, string>;
    type: EquipmentType;
    typeDescription: Record<string, string> | null;
    attributes: EquipmentsAttributes[];
    equipableDigimon: string[];
    note?: Record<string, string>;
}
