import { ImageCatalog } from "@/catalogs/image.catalog";
import type { MapRaw } from "@/repositories/tables/raws/map/map.raw";
import type { MapViewModel } from "@/viewmodels/map/map.viewmodel";

export class MapConverter {
  public static convert(locationRaw: MapRaw): MapViewModel {
    return {
      locationRegion: locationRaw.region ?? "asukaServer",
      locationImageUrl: ImageCatalog.getLocationImageUrl(locationRaw.imageName),
    };
  }
}
