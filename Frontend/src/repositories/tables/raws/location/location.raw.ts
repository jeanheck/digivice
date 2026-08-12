import type { LocationRegionConstant } from "@/constants/location-region.constant";

export interface LocationEnemyPhaseRaw {
  lastMainQuestStepDone: number;
  ids: string[];
}

export type LocationWalkingEnemiesRaw = string[] | LocationEnemyPhaseRaw[];

export interface LocationEnemiesRaw {
  walking?: LocationWalkingEnemiesRaw;
  fishing?: string[];
  kickingTree?: string[];
}

export interface LocationRaw {
  imageName: string;
  enemies?: LocationEnemiesRaw;
  region?: LocationRegionConstant;
  dock?: boolean;
}

export function isLocationEnemyPhaseList(
  walkingEnemies: LocationWalkingEnemiesRaw,
): walkingEnemies is LocationEnemyPhaseRaw[] {
  if (walkingEnemies.length === 0) {
    return false;
  }

  const firstEntry = walkingEnemies[0];
  return typeof firstEntry === "object" && firstEntry !== null && "ids" in firstEntry;
}
