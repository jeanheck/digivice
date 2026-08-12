import type { AsukaServerMapViewModel } from "@/viewmodels/map/asuka-server-map.viewmodel";

export class AsukaServerMapConverter {
  public static convert(
    locationId: string,
    enemyIds: string[],
    fishingIds: string[],
    kickingTreeIds: string[],
  ): AsukaServerMapViewModel {
    return {
      locationId,
      enemies: enemyIds,
      fishing: fishingIds,
      kickingTree: kickingTreeIds,
    };
  }
}
