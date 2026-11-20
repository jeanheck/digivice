import type { DigimonElement } from "@/shared/constants/digimon-element.constant";

export interface EnemyAffinityViewModel {
  element: DigimonElement;
  iconUrl: string | null;
}

export interface EnemyAffinitiesViewModel {
  effective: EnemyAffinityViewModel[];
  notEffective: EnemyAffinityViewModel[];
}
