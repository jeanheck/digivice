import type { EquipmentAttributeRaw } from "./equipment-attribute.raw";

export interface EquipmentRaw {
  type: string;
  imageName?: string;
  price?: number;
  attributes: EquipmentAttributeRaw[];
  equipableDigimon: string[];
}
