import type { AsukaServerMapViewModel } from "@/viewmodels/map/asuka-server-map.viewmodel";

export class AsukaServerMapConverter {
  public static convert(locationId: string, enemyIds: string[]): AsukaServerMapViewModel {
    return {
      locationId,
      enemies: enemyIds,
    };
  }
}
