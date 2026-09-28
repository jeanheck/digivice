import type { MapRegion } from "@/types/map-region.type";
import type { InnerLocationRaw } from "@/repositories/tables/raws/map/inner-location.raw";
import type { MapBossRaw } from "@/repositories/tables/raws/map/map-boss.raw";
import type { MapDuelIslandRaw } from "@/repositories/tables/raws/map/map-duel-island.raw";
import type { MapNpcRaw } from "@/repositories/tables/raws/map/map-npc.raw";
import type { MapCardShopRaw } from "@/repositories/tables/raws/map/map-card-shop.raw";
import type { MapTamerRaw } from "@/repositories/tables/raws/map/map-tamer.raw";
import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";

export interface MapEnemyPhaseRaw {
  lastMainQuestStepDone: number;
  ids: string[];
}

export type MapWalkingEnemiesRaw = string[] | MapEnemyPhaseRaw[];

export interface MapEnemiesRaw {
  walking?: MapWalkingEnemiesRaw;
  boss?: MapBossRaw[];
  fishing?: string[];
  kickingTree?: string[];
}

export interface MapRaw {
  imageName: string;
  worldLocation?: CoordinatesRaw;
  innerLocation?: InnerLocationRaw[];
  enemies?: MapEnemiesRaw;
  npcs?: MapNpcRaw[];
  tamers?: MapTamerRaw[];
  cardShops?: MapCardShopRaw[];
  duelIsland?: MapDuelIslandRaw[];
  region?: MapRegion;
  dock?: boolean;
}

export function isMapEnemyPhaseList(
  walkingEnemies: MapWalkingEnemiesRaw,
): walkingEnemies is MapEnemyPhaseRaw[] {
  if (walkingEnemies.length === 0) {
    return false;
  }

  const firstEntry = walkingEnemies[0];
  return typeof firstEntry === "object" && firstEntry !== null && "ids" in firstEntry;
}
