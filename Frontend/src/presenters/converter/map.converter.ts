import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import { LocationRegionConstant } from "@/constants/location-region.constant";
import type { LocationRaw } from "@/repositories/tables/raws/location/location.raw";
import type { MapViewModel } from "@/viewmodels/map/map.viewmodel";

export class MapConverter {
  public static convert(locationRaw: LocationRaw): MapViewModel {
    return {
      locationRegion: locationRaw.region ?? LocationRegionConstant.asukaServer,
      locationImageUrl: ImageCatalog.getLocationImageUrl(locationRaw.imageName),
    };
  }
}
