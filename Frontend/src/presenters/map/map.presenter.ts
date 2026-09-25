import { MapIdConstant } from "@/constants/map-id.constant";
import { MapConverter } from "@/presenters/converter/map.converter";
import { LocationRepository } from "@/repositories/location.repository";
import type { MapViewModel } from "@/viewmodels/map/map.viewmodel";

export class MapPresenter {
  public static getByLocationId(id: string): MapViewModel {
    return MapConverter.convert(LocationRepository.getLocationById(id));
  }

  public static isInBattle(locationId: string): boolean {
    return locationId === MapIdConstant.digimonBattle;
  }

  public static isInCardBattle(locationId: string): boolean {
    return locationId === MapIdConstant.cardBattle;
  }
}
