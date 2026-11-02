import type { DigimonStat } from "@/shared/types/digimon-stat.type";

export interface WikiEquipmentShopItemAttributeViewModel {
  attribute: DigimonStat;
  type: string;
  value: number;
  iconUrl: string | null;
}
