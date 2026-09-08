import type { EquipmentAttributeRaw } from "./equipment-attribute.raw";

export interface EquipmentDroppedByRaw {
  kind: "enemy";
  id: string;
  locationOnly?: string;
}

export interface EquipmentRaw {
  type: string;
  attributes: EquipmentAttributeRaw[];
  equipableDigimon: string[];
  droppedBy?: EquipmentDroppedByRaw[];
}
