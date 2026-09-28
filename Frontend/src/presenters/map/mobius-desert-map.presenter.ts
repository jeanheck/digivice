import type { Quest } from "@/models";
import { MapService } from "@/services/map.service";
import { MobiusDesertService } from "@/services/mobius-desert.service";
import { QuestService } from "@/services/quest.service";
import type { DesertAreaMapCellViewModel } from "@/viewmodels/desert/desert-area-map-cell.viewmodel";

export class MobiusDesertMapPresenter {
  public static getEnemyIds(locationId: string, mainQuest: Quest): string[] {
    const walkingIds = MapService.getWalkingEnemies(
      locationId,
      QuestService.getLastCompletedMainQuestStep(mainQuest),
    );
    const bossIds = MapService.getBoss(locationId);

    return [...bossIds, ...walkingIds];
  }

  public static getMobiusDesertArea(
    locationId: string,
    mapVariant: number | null,
  ): DesertAreaMapCellViewModel | null {
    return MobiusDesertService.getMobiusDesertArea(locationId, mapVariant);
  }
}
