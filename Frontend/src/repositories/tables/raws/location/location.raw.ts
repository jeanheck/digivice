import type { LocationRegionConstant } from "@/constants/location-region.constant";

export interface LocationEnemyPhaseRaw {
  lastMainQuestStepDone: number;
  ids: string[];
}

export type LocationEnemiesRaw = string[] | LocationEnemyPhaseRaw[];

export interface LocationRaw {
  image: string;
  enemies: LocationEnemiesRaw;
  region?: LocationRegionConstant;
  dock?: boolean;
}

export function isLocationEnemyPhaseList(enemies: LocationEnemiesRaw): enemies is LocationEnemyPhaseRaw[] {
  if (enemies.length === 0) {
    return false;
  }

  const firstEntry = enemies[0];
  return typeof firstEntry === "object" && firstEntry !== null && "ids" in firstEntry;
}
