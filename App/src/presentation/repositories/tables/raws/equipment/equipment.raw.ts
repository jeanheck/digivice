import type { EquipmentAttributeRaw } from "./equipment-attribute.raw";

export interface EquipmentRaw {
  type: string;
  imageName?: string;
  attributes: EquipmentAttributeRaw[];
  equipableDigimon: string[];
}
