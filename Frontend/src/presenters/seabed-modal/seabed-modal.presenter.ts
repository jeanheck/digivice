import { ImageCatalog } from "@/catalogs/image.catalog";
import { DockConverter } from "@/presenters/converter/dock.converter";
import { LocationConverter } from "@/presenters/converter/location.converter";
import { LocationRepository } from "@/repositories";
import { SeabedDockRepository } from "@/repositories/seabed-dock.repository";
import { LocationService } from "@/services/location.service";
import type { DockViewModel } from "@/viewmodels/dock/dock.viewmodel";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class SeabedModalPresenter {
  private static getDock(location: LocationViewModel | null): DockViewModel | null {
    if (location === null || location.dock === false) {
      return null;
    }

    const dockRaw = SeabedDockRepository.getDockByLocationId(location.id);

    if (dockRaw === null) {
      return null;
    }

    const imageUrl = ImageCatalog.getLocationImageUrl(location.image);

    return DockConverter.convert(dockRaw, imageUrl);
  }

  public static getDockByLocationId(
    locationId: string | null,
    seabedRoute: number = 0,
  ): DockViewModel | null {
    if (locationId === null) {
      return null;
    }

    const locationRaw = LocationRepository.getLocationById(locationId);
    const enemyIds = LocationService.getSeabedEnemies(seabedRoute);
    const location = LocationConverter.convert(locationId, locationRaw, enemyIds);

    return SeabedModalPresenter.getDock(location);
  }
}
