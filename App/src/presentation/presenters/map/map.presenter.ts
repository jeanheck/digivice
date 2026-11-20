import { MapId } from "@/shared/constants/map-id.constant";
import { MapConverter } from "@/presentation/presenters/converter/map.converter";
import { MapRepository } from "@/presentation/repositories/map.repository";
import type { MapViewModel } from "@/presentation/viewmodels/map/map.viewmodel";

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
