import type { DigimonStat } from "@/shared/types/digimon-stat.type";

export interface EquipmentAttributeRaw {
  attribute: DigimonStat;
  type: string;
  value: number;
}
