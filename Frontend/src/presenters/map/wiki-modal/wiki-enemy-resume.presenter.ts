import type { Quest } from "@/models";
import { LocationService } from "@/services/location.service";
import { QuestService } from "@/services/quest.service";
import type { EnemyLocationViewModel } from "@/viewmodels/enemy/enemy-location.viewmodel";

export class WikiEnemyResumePresenter {
  public static getResolvedEnemyLocations(
    locations: EnemyLocationViewModel[] | undefined,
    mainQuest: Quest | null,
  ): EnemyLocationViewModel[] {
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const resolvedLocations = (locations ?? []).filter((location) => {
      return LocationService.isLocationAvailableAccordingMainQuest(
        lastCompletedMainQuestStep,
        location.mainQuestStepDone,
      );
    });

    return [...resolvedLocations].sort((first, second) => {
      return first.id.localeCompare(second.id);
    });
  }
}
