import type { Quest } from "@/models";
import { LocationConverter } from "@/presenters/converter/location.converter";
import { LocationRepository } from "@/repositories/location.repository";
import { LocationService } from "@/services/location.service";
import { MobiusDesertService } from "@/services/mobius-desert.service";
import { QuestService } from "@/services/quest.service";
import type { DesertAreaMapCellViewModel } from "@/viewmodels/desert/desert-area-map-cell.viewmodel";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class MobiusDesertMapPresenter {
  public static getLocation(locationId: string, mainQuest: Quest | null): LocationViewModel {
    const locationRaw = LocationRepository.getLocationById(locationId);
    const enemyIds = LocationService.getEnemies(
      locationId,
      QuestService.getLastCompletedMainQuestStep(mainQuest),
    );

    return LocationConverter.convert(locationId, locationRaw, enemyIds);
  }

  public static getCell(locationId: string, mapVariant: number): DesertAreaMapCellViewModel | null {
    return MobiusDesertService.getCell(locationId, mapVariant);
  }
}
