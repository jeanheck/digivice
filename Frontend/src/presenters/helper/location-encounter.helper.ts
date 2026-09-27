import { MapId } from "@/constants/map-id.constant";
import type { Quest } from "@/models";
import { QuestRepository } from "@/repositories/quest.repository";
import { LocationService } from "@/services/location.service";
import { QuestService } from "@/services/quest.service";

export class LocationEncounterHelper {
  private static readonly FishingPoleQuestId = "fishingPole";
  private static readonly TreeBootsQuestId = "treeBoots";

  public static isAsukaSewersSafeZone(locationId: string, previousMapId: string): boolean {
    return (
      locationId === MapId.asukaSewers &&
      previousMapId === MapId.undergroundPath
    );
  }

  public static resolveWalkingIds(
    locationId: string,
    mainQuest: Quest,
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
      return quest.id === this.FishingPoleQuestId;
    });
    const fishingPoleRaw = QuestRepository.getSideQuestsRaw().find((questRaw) => {
      return questRaw.id === this.FishingPoleQuestId;
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
      return quest.id === this.TreeBootsQuestId;
    });
    const treeBootsRaw = QuestRepository.getSideQuestsRaw().find((questRaw) => {
      return questRaw.id === this.TreeBootsQuestId;
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
