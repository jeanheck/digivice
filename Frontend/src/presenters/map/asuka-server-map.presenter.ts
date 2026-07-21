import type { Quest } from "@/models";
import { AsukaServerMapConverter } from "@/presenters/converter/asuka-server-map.converter";
import { LocationService } from "@/services/location.service";
import type { AsukaServerMapViewModel } from "@/viewmodels/map/asuka-server-map.viewmodel";

export class AsukaServerMapPresenter {
  public static getViewModel(
    locationId: string,
    mainQuest: Quest | null,
    previousMapId: string = "",
  ): AsukaServerMapViewModel {
    return AsukaServerMapConverter.convert(
      locationId, 
      LocationService.getAsukaServerEnemies(locationId, mainQuest, previousMapId)
    );
  }
}
