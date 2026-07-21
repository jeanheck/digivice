import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import { LocationRegionConstant } from "@/constants/location-region.constant";
import { LocationService } from "@/services/location.service";

export class MapPresenter {
  public static getRegionByLocationId(id: string | null): LocationRegionConstant {
    return LocationService.getRegionByLocationId(id);
  }

  public static getLocationImageUrlByLocationId(id: string | null): string | null {
    return ImageCatalog.getLocationImageUrl(LocationService.getLocationImageNameByLocationId(id));
  }
}
