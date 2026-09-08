import type { Quest } from "@/models";
import { WikiLocationConverter } from "@/presenters/converter/wiki-location.converter";
import { LocationService } from "@/services/location.service";
import { QuestService } from "@/services/quest.service";
import type { EnemyLocationViewModel } from "@/viewmodels/enemy/enemy-location.viewmodel";
import type { WikiLocationViewModel } from "@/viewmodels/wiki-modal/wiki-location.viewmodel";

export class WikiEnemyResumePresenter {
  public static getResolvedEnemyLocations(
    locations: EnemyLocationViewModel[] | undefined,
    mainQuest: Quest | null,
  ): WikiLocationViewModel[] {
    const lastCompletedMainQuestStep = QuestService.getLastCompletedMainQuestStep(mainQuest);
    const resolvedLocations = (locations ?? []).filter((location) => {
      return LocationService.isLocationAvailableAccordingMainQuest(
        lastCompletedMainQuestStep,
        location.mainQuestStepDone,
      );
    });
    const sortedEnemyLocations = [...resolvedLocations].sort((first, second) => {
      return first.id.localeCompare(second.id);
    });

    return sortedEnemyLocations.map((location) => {
      return WikiLocationConverter.convert(location);
    });
  }
}
