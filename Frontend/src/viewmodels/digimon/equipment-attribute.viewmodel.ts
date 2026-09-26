import type { DigimonStat } from "@/types/digimon-stat.type";

export interface EquipmentAttributeViewModel {
  attribute: DigimonStat;
  type: string;
  value: number;
}
