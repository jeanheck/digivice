import type { Quest } from "@/models";
import { QuestService } from "@/services/quest.service";
import type { EnemyLocationViewModel } from "@/viewmodels/enemy/enemy-location.viewmodel";

export class WikiEnemyResumePresenter {
  public static getAvailableEnemyLocations(
    locations: EnemyLocationViewModel[] | undefined,
    mainQuest: Quest,
  ): EnemyLocationViewModel[] {
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const resolvedLocations = (locations ?? []).filter((location) => {
      return QuestService.isOnMainQuestRange(
        lastCompletedMainQuestStep,
        location.mainQuestAvailabilityWindow,
      );
    });

    return [...resolvedLocations].sort((first, second) => {
      return first.id.localeCompare(second.id);
    });
  }
}
