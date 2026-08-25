import type { AsukaServerMapViewModel } from "@/viewmodels/map/asuka-server-map.viewmodel";
import type { MapNpcViewModel } from "@/viewmodels/map/map-npc.viewmodel";

export class AsukaServerMapConverter {
  public static convert(
    locationId: string,
    enemyIds: string[],
    bossIds: string[],
    fishingIds: string[],
    kickingTreeIds: string[],
    npcs: MapNpcViewModel[],
  ): AsukaServerMapViewModel {
    return {
      locationId,
      enemies: enemyIds,
      boss: bossIds,
      fishing: fishingIds,
      kickingTree: kickingTreeIds,
      npcs,
    };
  }
}
