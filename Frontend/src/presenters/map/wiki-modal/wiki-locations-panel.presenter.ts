import { ImageCatalog } from "@/catalogs/image.catalog";
import type { Quest } from "@/models";
import { MapFrameSlideConverter } from "@/presenters/converter/map-frame-slide.converter";
import { WikiLocationConverter } from "@/presenters/converter/wiki-location.converter";
import { MainQuestRangeHelper } from "@/presenters/helper/main-quest-range.helper";
import { LocationRepository } from "@/repositories/location.repository";
import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";
import { QuestService } from "@/services/quest.service";
import type { EnemyLocationViewModel } from "@/viewmodels/enemy/enemy-location.viewmodel";
import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";
import type { WikiLocationViewModel } from "@/viewmodels/wiki-modal/wiki-location.viewmodel";
import type { WikiLocationsPanelViewModel } from "@/viewmodels/wiki-modal/wiki-locations-panel.viewmodel";

export class WikiLocationsPanelPresenter {
  public static getResolvedEnemyLocations(
    locations: EnemyLocationViewModel[] | undefined,
    mainQuest: Quest | null,
  ): WikiLocationViewModel[] {
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const resolvedLocations = WikiLocationsPanelPresenter.resolveLocationsByMainQuestRange(
      locations ?? [],
      lastCompletedMainQuestStep,
    );
    const sortedEnemyLocations = [...resolvedLocations].sort((first, second) => {
      return first.id.localeCompare(second.id);
    });

    return sortedEnemyLocations.map((location) => {
      return WikiLocationConverter.convert(location);
    });
  }

  public static getLocationPanelViewModel(locationId: string): WikiLocationsPanelViewModel {
    const locationRaw = LocationRepository.getLocationById(locationId);
    const worldLocation = WikiLocationsPanelPresenter.toCoordinates(locationRaw.worldLocation);
    const localImageUrl = ImageCatalog.getLocationImageUrl(locationRaw.imageName);

    return {
      asukaSlides: WikiLocationsPanelPresenter.getSlides(
        ImageCatalog.getLocationImageUrl("Asuka"),
        worldLocation,
      ),
      localSlides: WikiLocationsPanelPresenter.getSlides(localImageUrl, null),
      selectedLocationLabelKey: `location.${locationId}`,
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

  private static resolveLocationsByMainQuestRange(
    locations: EnemyLocationViewModel[],
    lastCompletedMainQuestStep: number,
  ): EnemyLocationViewModel[] {
    const locationsById = new Map<string, EnemyLocationViewModel[]>();

    for (const location of locations) {
      const existingLocations = locationsById.get(location.id) ?? [];
      existingLocations.push(location);
      locationsById.set(location.id, existingLocations);
    }

    const resolvedLocations: EnemyLocationViewModel[] = [];

    for (const groupedLocations of locationsById.values()) {
      if (groupedLocations.length === 1) {
        const uniqueLocation = groupedLocations[0];
        if (uniqueLocation !== undefined) {
          resolvedLocations.push(uniqueLocation);
        }
        continue;
      }

      const matchingLocation = WikiLocationsPanelPresenter.pickLocationInMainQuestRange(
        groupedLocations,
        lastCompletedMainQuestStep,
      );
      if (matchingLocation !== null) {
        resolvedLocations.push(matchingLocation);
      }
    }

    return resolvedLocations;
  }

  private static pickLocationInMainQuestRange(
    locations: EnemyLocationViewModel[],
    lastCompletedMainQuestStep: number,
  ): EnemyLocationViewModel | null {
    const locationsInRange = locations.filter((location) => {
      return WikiLocationsPanelPresenter.isInMainQuestRange(location, lastCompletedMainQuestStep);
    });

    if (locationsInRange.length === 0) {
      return null;
    }

    const sortedByStart = [...locationsInRange].sort((first, second) => {
      return (
        MainQuestRangeHelper.parseStart(first.startWhenLastMainQuestStepDone) -
        MainQuestRangeHelper.parseStart(second.startWhenLastMainQuestStepDone)
      );
    });

    return sortedByStart[0] ?? null;
  }

  private static isInMainQuestRange(
    location: EnemyLocationViewModel,
    lastCompletedMainQuestStep: number,
  ): boolean {
    return MainQuestRangeHelper.isInMainQuestRange(
      location.startWhenLastMainQuestStepDone,
      location.finishWhenLastMainQuestStepDone,
      lastCompletedMainQuestStep,
    );
  }
}
