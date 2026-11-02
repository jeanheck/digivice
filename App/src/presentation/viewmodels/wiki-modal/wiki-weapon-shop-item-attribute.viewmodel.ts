import type { DigimonStat } from "@/shared/types/digimon-stat.type";

export interface WikiWeaponShopItemAttributeViewModel {
  attribute: DigimonStat;
  type: string;
  value: number;
  iconUrl: string | null;
}
