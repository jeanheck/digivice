import type { EquipamentType } from '../equipament-type';
import type { EquipamentsAttributes } from '../equipaments-attributes';

export interface EnrichedEquipment {
    id: number;
    name: Record<string, string>;
    type: EquipamentType;
    typeDescription: Record<string, string> | null;
    attributes: EquipamentsAttributes[];
    equipableDigimon: string[];
    note?: Record<string, string>;
}
