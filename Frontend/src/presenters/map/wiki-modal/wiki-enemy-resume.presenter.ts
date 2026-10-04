import type { Party, Quest } from "@/models";
import { ImageCatalog } from "@/catalogs/image.catalog";
import { DigimonRepository } from "@/repositories";
import { QuestService } from "@/services/quest.service";
import type { EnemyLocationViewModel } from "@/viewmodels/enemy/enemy-location.viewmodel";
import type { EnemyPartyDvexpViewModel } from "@/viewmodels/enemy/enemy-party-dvexp.viewmodel";

const DvexpMultiplier = 10;
const DvexpLevelCap = 50;
const MinimumDvexp = 1;
const MaximumDvexp = 10;

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

  public static getPartyDvexp(party: Party, enemyBaseDvexp: number): EnemyPartyDvexpViewModel[] {
    return party.slots
      .filter((slot) => slot.digimonId !== null)
      .map((slot) => {
        const digimonId = slot.digimonId!;
        const digimonName = DigimonRepository.getNameById(digimonId);

        return {
          digimonId,
          digimonName,
          imageUrl: ImageCatalog.getDigimonImageUrl(`${digimonName}-healthy`),
          dvexp: this.calculateDvexp(enemyBaseDvexp, slot.digimon!.level),
        };
      });
  }

  private static calculateDvexp(enemyBaseDvexp: number, digimonLevel: number): number {
    const dvexp = Math.floor(
      (DvexpMultiplier * enemyBaseDvexp) / Math.min(digimonLevel, DvexpLevelCap),
    );

    return Math.min(MaximumDvexp, Math.max(MinimumDvexp, dvexp));
  }
}
