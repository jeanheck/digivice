import { MapIdConstant } from "@/constants/map-id.constant";
import type { Quest } from "@/models";
import { QuestRepository } from "@/repositories/quest.repository";
import { LocationService } from "@/services/location.service";
import { QuestService } from "@/services/quest.service";

export class LocationEncounterHelper {
  private static readonly FISHING_POLE_QUEST_ID = "fishingPole";
  private static readonly TREE_BOOTS_QUEST_ID = "treeBoots";

  public static isAsukaSewersSafeZone(locationId: string, previousMapId: string): boolean {
    return (
      locationId === MapIdConstant.asukaSewers &&
      previousMapId === MapIdConstant.undergroundPath
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
    return LocationService.getWalkingEnemies(locationId, lastCompletedMainQuestStep);
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

    return LocationService.getFishingEnemies(locationId);
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

    return LocationService.getKickingTreeEnemies(locationId);
  }
}
