import { ImageCatalog } from "@/catalogs/image.catalog";
import { SeabedModalConverter } from "@/presenters/converter/dock.converter";
import { LocationRepository } from "@/repositories";
import { SeabedDockRepository } from "@/repositories/seabed-dock.repository";
import type { SeabedModalViewModel } from "@/viewmodels/seabed-modal/seabed-modal.viewmodel";

export class SeabedModalPresenter {
  public static getInitialSelectedLocationId(playerLocationId: string | null): string | null {
    if (playerLocationId === null) {
      return null;
    }

    const locationRaw = LocationRepository.getLocationById(playerLocationId);
    if (locationRaw.dock !== true) {
      return null;
    }

    return playerLocationId;
  }

  public static getViewModel(locationId: string | null): SeabedModalViewModel | null {
    if (locationId === null) {
      return null;
    }

    const locationRaw = LocationRepository.getLocationById(locationId);
    if (locationRaw.dock !== true) {
      return null;
    }

    const seabedDockRaw = SeabedDockRepository.getDockByLocationId(locationId);
    if (seabedDockRaw === null) {
      return null;
    }

    const imageUrl = ImageCatalog.getLocationImageUrl(locationRaw.imageName);

    return SeabedModalConverter.convert(seabedDockRaw, imageUrl);
  }
}
