import { LocationRegionConstant } from "@/constants/location-region.constant";
import type { LocationRaw } from "@/repositories/tables/raws/location/location.raw";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class LocationConverter {
  public static convert(
    locationId: string,
    locationRaw: LocationRaw,
    resolvedEnemyIds: string[],
  ): LocationViewModel {
    return {
      id: locationId,
      image: locationRaw.imageName,
      enemies: resolvedEnemyIds,
      region: locationRaw.region ?? LocationRegionConstant.asukaServer,
      dock: locationRaw.dock === true,
    };
  }
}
