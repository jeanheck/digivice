import type { Quest } from "@/models";
import { AsukaServerMapConverter } from "@/presenters/converter/asuka-server-map.converter";
import { LocationService } from "@/services/location.service";
import { QuestService } from "@/services/quest.service";
import type { AsukaServerMapViewModel } from "@/viewmodels/map/asuka-server-map.viewmodel";

export class AsukaServerMapPresenter {
  private static readonly ASUKA_SEWERS_LOCATION_ID = "021B";
  private static readonly UNDERGROUND_PATH_LOCATION_ID = "020B";

  private static isAsukaSewersSafeZone(locationId: string, previousMapId: string): boolean {
    return locationId === this.ASUKA_SEWERS_LOCATION_ID && previousMapId === this.UNDERGROUND_PATH_LOCATION_ID;
  }

  public static getViewModel(
    locationId: string,
    mainQuest: Quest | null,
    previousMapId: string = "",
  ): AsukaServerMapViewModel {
    if (this.isAsukaSewersSafeZone(locationId, previousMapId)) {
      return AsukaServerMapConverter.convert(locationId, []);
    }

    const enemyIds = LocationService.getEnemies(
      locationId,
      QuestService.getLastCompletedMainQuestStep(mainQuest),
    );

    return AsukaServerMapConverter.convert(locationId, enemyIds);
  }
}
