import { ImageCatalog } from "@/catalogs/image.catalog";
import { DockConverter } from "@/presenters/converter/dock.converter";
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
}
