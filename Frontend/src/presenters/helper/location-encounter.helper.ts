import type { Quest } from "@/models";
import { QuestRepository } from "@/repositories/quest.repository";
import { LocationService } from "@/services/location.service";
import { QuestService } from "@/services/quest.service";

export class LocationEncounterHelper {
  private static readonly ASUKA_SEWERS_LOCATION_ID = "021B";
  private static readonly UNDERGROUND_PATH_LOCATION_ID = "020B";
  private static readonly FISHING_POLE_QUEST_ID = "fishingPole";
  private static readonly TREE_BOOTS_QUEST_ID = "treeBoots";

  public static isAsukaSewersSafeZone(locationId: string, previousMapId: string): boolean {
    return (
      locationId === this.ASUKA_SEWERS_LOCATION_ID &&
      previousMapId === this.UNDERGROUND_PATH_LOCATION_ID
    );
  }

  public static resolveWalkingIds(
    locationId: string,
    mainQuest: Quest | null,
    previousMapId: string,
  ): string[] {
    if (this.isAsukaSewersSafeZone(locationId, previousMapId)) {
      return [];
    }

    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    return LocationService.getEnemies(locationId, lastCompletedMainQuestStep);
  }

  public static resolveFishingIds(locationId: string, sideQuests: Quest[]): string[] {
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

  public static resolveKickingTreeIds(locationId: string, sideQuests: Quest[]): string[] {
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
}
