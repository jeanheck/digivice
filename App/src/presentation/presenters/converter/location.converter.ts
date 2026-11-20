import type { MapRaw } from "@/presentation/repositories/tables/raws/map/map.raw";
import type { LocationViewModel } from "@/presentation/viewmodels/location/location.viewmodel";

export class LocationConverter {
  public static convert(
    locationId: string,
    locationRaw: MapRaw,
    resolvedEnemyIds: string[],
  ): LocationViewModel {
    return {
      id: locationId,
      image: locationRaw.imageName,
      enemies: resolvedEnemyIds,
      region: locationRaw.region ?? "asukaServer",
      dock: locationRaw.dock === true,
    };
  }
}
