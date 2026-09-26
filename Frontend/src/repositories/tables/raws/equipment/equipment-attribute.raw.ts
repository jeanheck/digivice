import type { DigimonStat } from "@/types/digimon-stat.type";

export interface EquipmentAttributeRaw {
  attribute: DigimonStat;
  type: string;
  value: number;
}
