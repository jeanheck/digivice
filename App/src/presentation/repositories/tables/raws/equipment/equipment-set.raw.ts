import type { EquipmentAttributeRaw } from "./equipment-attribute.raw";

export interface EquipmentSetRaw {
  equipments: number[];
  attributes: EquipmentAttributeRaw[];
}
