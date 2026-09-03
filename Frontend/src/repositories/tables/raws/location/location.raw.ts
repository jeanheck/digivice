import type { LocationRegionConstant } from "@/constants/location-region.constant";
import type { InnerLocationRaw } from "@/repositories/tables/raws/location/inner-location.raw";
import type { LocationBossRaw } from "@/repositories/tables/raws/location/location-boss.raw";
import type { LocationDuelIslandRaw } from "@/repositories/tables/raws/location/location-duel-island.raw";
import type { LocationNpcRaw } from "@/repositories/tables/raws/location/location-npc.raw";
import type { LocationStoreRaw } from "@/repositories/tables/raws/location/location-store.raw";
import type { LocationTamerRaw } from "@/repositories/tables/raws/location/location-tamer.raw";
import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";

export interface LocationEnemyPhaseRaw {
  lastMainQuestStepDone: number;
  ids: string[];
}

export type LocationWalkingEnemiesRaw = string[] | LocationEnemyPhaseRaw[];

export interface LocationEnemiesRaw {
  walking?: LocationWalkingEnemiesRaw;
  boss?: LocationBossRaw[];
  fishing?: string[];
  kickingTree?: string[];
}

export interface LocationRaw {
  imageName: string;
  worldLocation?: CoordinatesRaw;
  innerLocation?: InnerLocationRaw[];
  enemies?: LocationEnemiesRaw;
  npcs?: LocationNpcRaw[];
  tamers?: LocationTamerRaw[];
  stores?: LocationStoreRaw[];
  duelIsland?: LocationDuelIslandRaw[];
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
