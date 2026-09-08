import { ImageCatalog } from "@/catalogs/image.catalog";
import type { Quest } from "@/models";
import { MapFrameSlideConverter } from "@/presenters/converter/map-frame-slide.converter";
import { LocationEncounterHelper } from "@/presenters/helper/location-encounter.helper";
import { NpcBattleOpponentHelper } from "@/presenters/helper/npc-battle-opponent.helper";
import { EnemyRepository } from "@/repositories/enemy.repository";
import { LocationBossRepository } from "@/repositories/location-boss.repository";
import { LocationRepository } from "@/repositories/location.repository";
import { LocationCardShopRepository } from "@/repositories/location-card-shop.repository";
import { CardShopRepository } from "@/repositories/card-shop.repository";
import type { LocationBossRaw } from "@/repositories/tables/raws/location/location-boss.raw";
import type { LocationDuelIslandRaw } from "@/repositories/tables/raws/location/location-duel-island.raw";
import type { LocationMapLabelPlacementRaw } from "@/repositories/tables/raws/location/location-map-label-placement.raw";
import type { LocationNpcRaw } from "@/repositories/tables/raws/location/location-npc.raw";
import type { LocationCardShopRaw } from "@/repositories/tables/raws/location/location-card-shop.raw";
import type { LocationTamerRaw } from "@/repositories/tables/raws/location/location-tamer.raw";
import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";
import { QuestService } from "@/services/quest.service";
import type { EnemyLocationSourceViewModel } from "@/viewmodels/enemy/enemy-location-source.viewmodel";
import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";
import type { WikiLocationEncounterEnemyViewModel } from "@/viewmodels/wiki-modal/wiki-location-encounter-enemy.viewmodel";
import type { WikiLocationEncounterLineViewModel } from "@/viewmodels/wiki-modal/wiki-location-encounter-line.viewmodel";
import type { WikiLocationMapLabelPlacementViewModel } from "@/viewmodels/wiki-modal/wiki-location-map-marker.viewmodel";
import type { WikiLocationMapMarkerViewModel } from "@/viewmodels/wiki-modal/wiki-location-map-marker.viewmodel";
import type { WikiLocationsPanelViewModel } from "@/viewmodels/wiki-modal/wiki-locations-panel.viewmodel";

export class WikiLocationsPanelPresenter {
  public static getLocationPanelViewModel(
    locationId: string,
    mainQuest: Quest | null,
    sideQuests: Quest[],
    previousMapId: string,
  ): WikiLocationsPanelViewModel {
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
      encounterLines: WikiLocationsPanelPresenter.getEncounterLines(
        locationId,
        mainQuest,
        sideQuests,
        previousMapId,
      ),
      mapMarkers: WikiLocationsPanelPresenter.getMapMarkers(locationId, mainQuest),
    };
  }

  public static getMapMarkers(
    locationId: string,
    mainQuest: Quest | null,
  ): WikiLocationMapMarkerViewModel[] {
    const locationRaw = LocationRepository.getLocationById(locationId);
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const markers: WikiLocationMapMarkerViewModel[] = [];

    for (const tamer of locationRaw.tamers ?? []) {
      const marker = WikiLocationsPanelPresenter.toMapMarker(tamer);
      if (marker !== null) {
        markers.push(marker);
      }
    }

    for (const duelIslandEntry of locationRaw.duelIsland ?? []) {
      const marker = WikiLocationsPanelPresenter.toMapMarker(duelIslandEntry);
      if (marker !== null) {
        markers.push(marker);
      }
    }

    for (const locationNpc of locationRaw.npcs ?? []) {
      if (
        !QuestService.isOnMainQuestRange(
          lastCompletedMainQuestStep,
          locationNpc.mainQuestStepDone,
        )
      ) {
        continue;
      }

      const marker = WikiLocationsPanelPresenter.toMapMarker(locationNpc);
      if (marker !== null) {
        markers.push(marker);
      }
    }

    for (const locationBoss of LocationBossRepository.getByLocationId(locationId)) {
      const marker = WikiLocationsPanelPresenter.toBossMapMarker(locationBoss);
      if (marker !== null) {
        markers.push(marker);
      }
    }

    for (const locationCardShop of LocationCardShopRepository.getByLocationId(locationId)) {
      const marker = WikiLocationsPanelPresenter.toCardShopMapMarker(locationCardShop);
      if (marker !== null) {
        markers.push(marker);
      }
    }

    return markers.sort((first, second) => {
      return first.id.localeCompare(second.id);
    });
  }

  private static toMapMarker(
    entry: LocationTamerRaw | LocationNpcRaw | LocationDuelIslandRaw,
  ): WikiLocationMapMarkerViewModel | null {
    if (entry.coordinates === undefined) {
      return null;
    }

    const nameKey = NpcBattleOpponentHelper.getNameKey(entry.id);
    if (nameKey === null) {
      return null;
    }

    return {
      id: entry.id,
      kind: "npc",
      nameKey,
      imageUrl: NpcBattleOpponentHelper.getImageUrl(entry.id),
      coordinates: WikiLocationsPanelPresenter.toCoordinates(entry.coordinates)!,
      labelPlacement: WikiLocationsPanelPresenter.toLabelPlacement(entry.labelPlacement),
    };
  }

  private static toBossMapMarker(entry: LocationBossRaw): WikiLocationMapMarkerViewModel | null {
    if (entry.coordinates === undefined) {
      return null;
    }

    const enemyRaw = EnemyRepository.getEnemyById(entry.id);

    return {
      id: entry.id,
      kind: "boss",
      name: enemyRaw.name,
      imageUrl: ImageCatalog.getBossImageUrl(enemyRaw.name),
      coordinates: WikiLocationsPanelPresenter.toCoordinates(entry.coordinates)!,
      labelPlacement: WikiLocationsPanelPresenter.toLabelPlacement(entry.labelPlacement),
    };
  }

  private static toCardShopMapMarker(entry: LocationCardShopRaw): WikiLocationMapMarkerViewModel | null {
    if (entry.coordinates === undefined) {
      return null;
    }

    const cardShopRaw = CardShopRepository.getById(entry.id);
    if (cardShopRaw === undefined) {
      return null;
    }

    return {
      id: entry.id,
      kind: "cardShop",
      nameKey: `cardShops.${entry.id}.name`,
      imageUrl: ImageCatalog.getCardShopImageUrl(cardShopRaw.imageName),
      coordinates: WikiLocationsPanelPresenter.toCoordinates(entry.coordinates)!,
      labelPlacement: WikiLocationsPanelPresenter.toLabelPlacement(entry.labelPlacement),
    };
  }

  private static toLabelPlacement(
    labelPlacement: LocationMapLabelPlacementRaw | undefined,
  ): WikiLocationMapLabelPlacementViewModel {
    if (labelPlacement === undefined) {
      return "below";
    }

    return labelPlacement;
  }

  private static getEncounterLines(
    locationId: string,
    mainQuest: Quest | null,
    sideQuests: Quest[],
    previousMapId: string,
  ): WikiLocationEncounterLineViewModel[] {
    const encounterSources: Exclude<EnemyLocationSourceViewModel, "boss">[] = [
      "walking",
      "fishing",
      "kickingTree",
    ];
    const enemyIdsBySource: Record<
      Exclude<EnemyLocationSourceViewModel, "boss">,
      string[]
    > = {
      walking: LocationEncounterHelper.resolveWalkingIds(locationId, mainQuest, previousMapId),
      fishing: LocationEncounterHelper.resolveFishingIds(locationId, sideQuests),
      kickingTree: LocationEncounterHelper.resolveKickingTreeIds(locationId, sideQuests),
    };

    const encounterLines = encounterSources.flatMap((source) => {
      const enemyIds = enemyIdsBySource[source];
      if (enemyIds.length === 0) {
        return [];
      }

      return [
        {
          source,
          enemies: enemyIds.map((enemyId) => {
            return WikiLocationsPanelPresenter.toEncounterEnemy(enemyId);
          }),
        },
      ];
    });

    return encounterLines;
  }

  private static toEncounterEnemy(enemyId: string): WikiLocationEncounterEnemyViewModel {
    const enemyRaw = EnemyRepository.getEnemyById(enemyId);

    return {
      id: enemyId,
      name: enemyRaw.name,
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
