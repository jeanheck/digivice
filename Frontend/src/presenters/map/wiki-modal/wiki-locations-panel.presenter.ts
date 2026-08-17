import { ImageCatalog } from "@/catalogs/image.catalog";
import { MapFrameSlideConverter } from "@/presenters/converter/map-frame-slide.converter";
import { WikiLocationConverter } from "@/presenters/converter/wiki-location.converter";
import { LocationRepository } from "@/repositories/location.repository";
import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";
import type { EnemyLocationViewModel } from "@/viewmodels/enemy/enemy-location.viewmodel";
import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";
import type { WikiLocationsPanelViewModel } from "@/viewmodels/wiki-modal/wiki-locations-panel.viewmodel";

export class WikiLocationsPanelPresenter {
  public static getViewModel(
    locations: EnemyLocationViewModel[] | undefined,
    selectedId: string | null,
  ): WikiLocationsPanelViewModel {
    const sortedEnemyLocations = [...(locations ?? [])].sort((first, second) => {
      return first.id.localeCompare(second.id);
    });

    const convertedLocations = sortedEnemyLocations.map((location) => {
      return WikiLocationConverter.convert(location);
    });

    const selectedEnemyLocation =
      sortedEnemyLocations.find((location) => {
        return location.id === selectedId;
      }) ?? null;

    let worldLocation: CoordinatesViewModel | null = null;
    let localImageUrl: string | null = null;

    if (selectedEnemyLocation !== null) {
      const locationRaw = LocationRepository.getLocationById(selectedEnemyLocation.id);
      worldLocation = WikiLocationsPanelPresenter.toCoordinates(locationRaw.worldLocation);
      localImageUrl = ImageCatalog.getLocationImageUrl(locationRaw.imageName);
    }

    return {
      locations: convertedLocations,
      asukaSlides: WikiLocationsPanelPresenter.getSlides(
        ImageCatalog.getLocationImageUrl("Asuka"),
        worldLocation,
      ),
      localSlides: WikiLocationsPanelPresenter.getSlides(
        localImageUrl,
        selectedEnemyLocation?.localCoordinates ?? null,
      ),
      selectedLocationLabelKey:
        selectedEnemyLocation === null ? null : `location.${selectedEnemyLocation.id}`,
    };
  }

  private static toCoordinates(
    coordinates: CoordinatesRaw | undefined,
  ): CoordinatesViewModel | null {
    if (coordinates === undefined) {
      return null;
    }

    return {
      x: coordinates.x,
      y: coordinates.y,
    };
  }

  private static getSlides(
    imageUrl: string | null,
    coordinates: CoordinatesViewModel | null,
  ): MapFrameSlideViewModel[] {
    if (imageUrl === null) {
      return [];
    }

    return [MapFrameSlideConverter.convert(imageUrl, coordinates)];
  }
}
