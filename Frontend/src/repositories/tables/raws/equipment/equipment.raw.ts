import type { EquipmentAttributeRaw } from "./equipment-attribute.raw";

export interface EquipmentRaw {
    type: string;
    attributes: EquipmentAttributeRaw[];
    equipableDigimon: string[];
}
