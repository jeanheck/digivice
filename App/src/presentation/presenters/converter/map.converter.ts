import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import type { MapRaw } from "@/presentation/repositories/tables/raws/map/map.raw";
import type { MapViewModel } from "@/presentation/viewmodels/map/map.viewmodel";

export class MapConverter {
  public static convert(locationRaw: MapRaw): MapViewModel {
    return {
      locationRegion: locationRaw.region ?? "asukaServer",
      locationImageUrl: ImageCatalog.getLocationImageUrl(locationRaw.imageName),
    };
  }
}
