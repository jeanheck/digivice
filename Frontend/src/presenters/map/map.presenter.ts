import { MapId } from "@/constants/map-id.constant";
import { MapConverter } from "@/presenters/converter/map.converter";
import { MapRepository } from "@/repositories/map.repository";
import type { MapViewModel } from "@/viewmodels/map/map.viewmodel";

export class MapPresenter {
  public static getByLocationId(id: string): MapViewModel {
    return MapConverter.convert(MapRepository.getMapById(id));
  }

  public static isInBattle(locationId: string): boolean {
    return locationId === MapId.digimonBattle;
  }

  public static isInCardBattle(locationId: string): boolean {
    return locationId === MapId.cardBattle;
  }
}
