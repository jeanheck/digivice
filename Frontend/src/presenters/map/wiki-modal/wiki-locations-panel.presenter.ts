import { ImageCatalog } from "@/catalogs/image.catalog";
import type { Quest } from "@/models";
import { MapFrameSlideConverter } from "@/presenters/converter/map-frame-slide.converter";
import { WikiLocationConverter } from "@/presenters/converter/wiki-location.converter";
import { LocationRepository } from "@/repositories/location.repository";
import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";
import { QuestService } from "@/services/quest.service";
import type { EnemyLocationViewModel } from "@/viewmodels/enemy/enemy-location.viewmodel";
import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";
import type { WikiLocationsPanelViewModel } from "@/viewmodels/wiki-modal/wiki-locations-panel.viewmodel";

export class WikiLocationsPanelPresenter {
  public static getViewModel(
    locations: EnemyLocationViewModel[] | undefined,
    selectedId: string | null,
    mainQuest: Quest | null,
  ): WikiLocationsPanelViewModel {
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const resolvedLocations = WikiLocationsPanelPresenter.resolveLocationsByMainQuestRange(
      locations ?? [],
      lastCompletedMainQuestStep,
    );
    const sortedEnemyLocations = [...resolvedLocations].sort((first, second) => {
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
        WikiLocationsPanelPresenter.parseStart(first.startWhenLastMainQuestStepDone) -
        WikiLocationsPanelPresenter.parseStart(second.startWhenLastMainQuestStepDone)
      );
    });

    return sortedByStart[0] ?? null;
  }

  private static isInMainQuestRange(
    location: EnemyLocationViewModel,
    lastCompletedMainQuestStep: number,
  ): boolean {
    const start = WikiLocationsPanelPresenter.parseStart(location.startWhenLastMainQuestStepDone);
    const finish = WikiLocationsPanelPresenter.parseFinish(location.finishWhenLastMainQuestStepDone);

    return lastCompletedMainQuestStep >= start && lastCompletedMainQuestStep <= finish;
  }

  private static parseStart(value: string | undefined): number {
    if (value === undefined || value === "") {
      return 0;
    }

    const parsedValue = Number(value);
    if (Number.isNaN(parsedValue)) {
      return 0;
    }

    return parsedValue;
  }

  private static parseFinish(value: string | undefined): number {
    if (value === undefined || value === "") {
      return Number.POSITIVE_INFINITY;
    }

    const parsedValue = Number(value);
    if (Number.isNaN(parsedValue)) {
      return Number.POSITIVE_INFINITY;
    }

    return parsedValue;
  }
}
