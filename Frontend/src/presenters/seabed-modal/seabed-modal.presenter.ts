import { ImageCatalog } from "@/catalogs/image.catalog";
import { SeabedModalConverter } from "@/presenters/converter/dock.converter";
import { LocationConverter } from "@/presenters/converter/location.converter";
import { LocationRepository } from "@/repositories";
import { SeabedDockRepository } from "@/repositories/seabed-dock.repository";
import { LocationService } from "@/services/location.service";
import type { SeabedModalViewModel } from "@/viewmodels/seabed-modal/seabed-modal.viewmodel";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class SeabedModalPresenter {
  private static getDock(location: LocationViewModel | null): SeabedModalViewModel | null {
    if (location === null || location.dock === false) {
      return null;
    }

    const seabedDockRaw = SeabedDockRepository.getDockByLocationId(location.id);

    if (seabedDockRaw === null) {
      return null;
    }

    const imageUrl = ImageCatalog.getLocationImageUrl(location.image);

    return SeabedModalConverter.convert(seabedDockRaw, imageUrl);
  }

  public static getSeabedModalViewModelByLocationId(
    locationId: string | null,
    seabedRoute: number = 0,
  ): SeabedModalViewModel | null {
    if (locationId === null) {
      return null;
    }

    const locationRaw = LocationRepository.getLocationById(locationId);
    const enemyIds = LocationService.getSeabedEnemies(seabedRoute);
    const location = LocationConverter.convert(locationId, locationRaw, enemyIds);

    return SeabedModalPresenter.getDock(location);
  }
}
