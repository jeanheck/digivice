import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import type { Journal, Quest } from "@/state/models";
import { MapFrameSlideConverter } from "@/presentation/presenters/converter/map-frame-slide.converter";
import { LocationEncounterHelper } from "@/presentation/presenters/helper/location-encounter.helper";
import { NpcBattleOpponentHelper } from "@/presentation/presenters/helper/npc-battle-opponent.helper";
import { EnemyRepository } from "@/presentation/repositories/enemy.repository";
import { MapBossRepository } from "@/presentation/repositories/map-boss.repository";
import { MapRepository } from "@/presentation/repositories/map.repository";
import { MapCardShopRepository } from "@/presentation/repositories/map-card-shop.repository";
import { NpcRepository } from "@/presentation/repositories/npc.repository";
import { CardShopRepository } from "@/presentation/repositories/card-shop.repository";
import type { MapBossRaw } from "@/presentation/repositories/tables/raws/map/map-boss.raw";
import type { MapDuelIslandRaw } from "@/presentation/repositories/tables/raws/map/map-duel-island.raw";
import type { MapNpcRaw } from "@/presentation/repositories/tables/raws/map/map-npc.raw";
import type { MapCardShopRaw } from "@/presentation/repositories/tables/raws/map/map-card-shop.raw";
import type { MapTamerRaw } from "@/presentation/repositories/tables/raws/map/map-tamer.raw";
import type { CoordinatesRaw } from "@/presentation/repositories/tables/raws/quest/coordinates.raw";
import { MomentService } from "@/presentation/services/moment.service";
import { QuestService } from "@/presentation/services/quest.service";
import type { EnemySource } from "@/shared/types/enemy-source.type";
import type { MapFrameSlideViewModel } from "@/presentation/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { CoordinatesViewModel } from "@/presentation/viewmodels/quest/coordinates.viewmodel";
import type { WikiLocationEncounterEnemyViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-location-encounter-enemy.viewmodel";
import type { WikiLocationEncounterLineViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-location-encounter-line.viewmodel";
import type { LabelPlacement } from "@/shared/types/label-placement.type";
import type { WikiLocationMapMarkerViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-location-map-marker.viewmodel";
import type { WikiLocationsPanelViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-locations-panel.viewmodel";

export class WikiLocationsPanelPresenter {
  public static getLocationPanelViewModel(
    locationId: string,
    journal: Journal,
    previousMapId: string,
  ): WikiLocationsPanelViewModel {
    const locationRaw = MapRepository.getMapById(locationId);
    const worldLocation = WikiLocationsPanelPresenter.toCoordinates(locationRaw.worldLocation);
    const localImageUrl = ImageCatalog.getLocationImageUrl(locationRaw.imageName);

    return {
      asukaSlides: WikiLocationsPanelPresenter.getSlides(
        ImageCatalog.getLocationImageUrl("Asuka"),
        worldLocation,
      ),
      localSlides: WikiLocationsPanelPresenter.getSlides(localImageUrl, null),
      selectedLocationLabelKey: `map.${locationId}`,
      encounterLines: WikiLocationsPanelPresenter.getEncounterLines(
        locationId,
        journal.mainQuest,
        journal.sideQuests,
        previousMapId,
      ),
      mapMarkers: WikiLocationsPanelPresenter.getMapMarkers(locationId, journal),
    };
  }

  public static getMapMarkers(
    locationId: string,
    journal: Journal,
  ): WikiLocationMapMarkerViewModel[] {
    const locationRaw = MapRepository.getMapById(locationId);
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(
      journal.mainQuest,
    );
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
          NpcRepository.getNpcById(locationNpc.id)?.mainQuestAvailabilityWindow,
        )
      ) {
        continue;
      }

      const marker = WikiLocationsPanelPresenter.toMapMarker(locationNpc);
      if (marker !== null) {
        markers.push(marker);
      }
    }

    for (const locationBoss of MapBossRepository.getByMapId(locationId)) {
      if (!MomentService.isActive(locationBoss.moment, journal)) {
        continue;
      }

      const marker = WikiLocationsPanelPresenter.toBossMapMarker(locationBoss);
      if (marker !== null) {
        markers.push(marker);
      }
    }

    for (const locationCardShop of MapCardShopRepository.getByMapId(locationId)) {
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
    entry: MapTamerRaw | MapNpcRaw | MapDuelIslandRaw,
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

  private static toBossMapMarker(entry: MapBossRaw): WikiLocationMapMarkerViewModel | null {
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

  private static toCardShopMapMarker(entry: MapCardShopRaw): WikiLocationMapMarkerViewModel | null {
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
      nameKey: `cardShop.${entry.id}.name`,
      imageUrl: ImageCatalog.getCardShopImageUrl(cardShopRaw.imageName),
      coordinates: WikiLocationsPanelPresenter.toCoordinates(entry.coordinates)!,
      labelPlacement: WikiLocationsPanelPresenter.toLabelPlacement(entry.labelPlacement),
    };
  }

  private static toLabelPlacement(labelPlacement: LabelPlacement | undefined): LabelPlacement {
    if (labelPlacement === undefined) {
      return "below";
    }

    return labelPlacement;
  }

  private static getEncounterLines(
    locationId: string,
    mainQuest: Quest,
    sideQuests: Quest[],
    previousMapId: string,
  ): WikiLocationEncounterLineViewModel[] {
    const encounterSources: Exclude<EnemySource, "boss">[] = ["walking", "fishing", "kickingTree"];
    const enemyIdsBySource: Record<Exclude<EnemySource, "boss">, string[]> = {
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
