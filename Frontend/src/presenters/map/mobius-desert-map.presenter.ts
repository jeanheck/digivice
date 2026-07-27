import type { Quest } from "@/models";
import { LocationService } from "@/services/location.service";
import { MobiusDesertService } from "@/services/mobius-desert.service";
import { QuestService } from "@/services/quest.service";
import type { DesertAreaMapCellViewModel } from "@/viewmodels/desert/desert-area-map-cell.viewmodel";

export class MobiusDesertMapPresenter {
  public static getEnemyIds(locationId: string, mainQuest: Quest | null): string[] {
    return LocationService.getEnemies(
      locationId,
      QuestService.getLastCompletedMainQuestStep(mainQuest),
    );
  }

  public static getMobiusDesertArea(locationId: string, mapVariant: number): DesertAreaMapCellViewModel | null {
    return MobiusDesertService.getMobiusDesertArea(locationId, mapVariant);
  }
}
