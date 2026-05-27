import type { EquipmentAttributeRaw } from "./equipment-attribute.raw";

export interface EquipmentRaw {
    name: Record<string, string>;
    type: string;
    attributes: EquipmentAttributeRaw[];
    equipableDigimon: string[];
}
