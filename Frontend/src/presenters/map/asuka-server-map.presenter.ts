import type { Quest } from "@/models";
import { AsukaServerMapConverter } from "@/presenters/converter/asuka-server-map.converter";
import { QuestRepository } from "@/repositories/quest.repository";
import { LocationService } from "@/services/location.service";
import { QuestService } from "@/services/quest.service";
import type { AsukaServerMapViewModel } from "@/viewmodels/map/asuka-server-map.viewmodel";

export class AsukaServerMapPresenter {
  private static readonly ASUKA_SEWERS_LOCATION_ID = "021B";
  private static readonly UNDERGROUND_PATH_LOCATION_ID = "020B";
  private static readonly FISHING_POLE_QUEST_ID = "fishingPole";
  private static readonly TREE_BOOTS_QUEST_ID = "treeBoots";

  private static isAsukaSewersSafeZone(locationId: string, previousMapId: string): boolean {
    return (
      locationId === this.ASUKA_SEWERS_LOCATION_ID &&
      previousMapId === this.UNDERGROUND_PATH_LOCATION_ID
    );
  }

  private static resolveFishingIds(locationId: string, sideQuests: Quest[]): string[] {
    const fishingPoleQuest = sideQuests.find((quest) => {
      return quest.id === this.FISHING_POLE_QUEST_ID;
    });
    const fishingPoleRaw = QuestRepository.getSideQuestsRaw().find((questRaw) => {
      return questRaw.id === this.FISHING_POLE_QUEST_ID;
    });

    if (
      fishingPoleRaw === undefined ||
      !QuestService.isQuestCompleted(fishingPoleQuest, fishingPoleRaw)
    ) {
      return [];
    }

    return LocationService.getFishing(locationId);
  }

  private static resolveKickingTreeIds(locationId: string, sideQuests: Quest[]): string[] {
    const treeBootsQuest = sideQuests.find((quest) => {
      return quest.id === this.TREE_BOOTS_QUEST_ID;
    });
    const treeBootsRaw = QuestRepository.getSideQuestsRaw().find((questRaw) => {
      return questRaw.id === this.TREE_BOOTS_QUEST_ID;
    });

    if (
      treeBootsRaw === undefined ||
      !QuestService.isQuestCompleted(treeBootsQuest, treeBootsRaw)
    ) {
      return [];
    }

    return LocationService.getKickingTree(locationId);
  }

  public static getViewModel(
    locationId: string,
    mainQuest: Quest | null,
    sideQuests: Quest[],
    previousMapId: string = "",
  ): AsukaServerMapViewModel {
    const fishingIds = this.resolveFishingIds(locationId, sideQuests);
    const kickingTreeIds = this.resolveKickingTreeIds(locationId, sideQuests);
    const bossIds = LocationService.getBoss(locationId);

    if (this.isAsukaSewersSafeZone(locationId, previousMapId)) {
      return AsukaServerMapConverter.convert(locationId, [], bossIds, fishingIds, kickingTreeIds);
    }

    const enemyIds = LocationService.getEnemies(
      locationId,
      QuestService.getLastCompletedMainQuestStep(mainQuest),
    );

    return AsukaServerMapConverter.convert(
      locationId,
      enemyIds,
      bossIds,
      fishingIds,
      kickingTreeIds,
    );
  }
}
