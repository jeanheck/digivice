import { ImageCatalog } from "@/catalogs/image.catalog";
import type { Quest } from "@/models";
import { DockConverter } from "@/presenters/converter/dock.converter";
import { MapPresenter } from "@/presenters/map/map.presenter";
import { DockRepository } from "@/repositories/dock.repository";
import type { DockViewModel } from "@/viewmodels/dock/dock.viewmodel";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class DocksSectionPresenter {
  public static getDock(location: LocationViewModel | null): DockViewModel | null {
    if (location === null || location.dock === false) {
      return null;
    }

    const dockRaw = DockRepository.getDockByLocationId(location.id);

    if (dockRaw === null) {
      return null;
    }

    const imageUrl = ImageCatalog.getMapImageUrl(location.image);

    return DockConverter.convert(dockRaw, imageUrl);
  }

  public static getDockByLocationId(
    locationId: string | null,
    mainQuest: Quest | null,
    seabedRoute: number = 0,
    previousMapId: string = "",
  ): DockViewModel | null {
    if (locationId === null) {
      return null;
    }

    const location = MapPresenter.getLocationById(locationId, mainQuest, seabedRoute, previousMapId);

    return DocksSectionPresenter.getDock(location);
  }
}
