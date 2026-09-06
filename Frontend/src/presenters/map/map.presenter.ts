import { MapConverter } from "@/presenters/converter/map.converter";
import { LocationRepository } from "@/repositories/location.repository";
import type { MapViewModel } from "@/viewmodels/map/map.viewmodel";

const BATTLE_LOCATION_ID = "0600";
const CARD_BATTLE_LOCATION_ID = "0700";

export class MapPresenter {
  public static getByLocationId(id: string | null): MapViewModel {
    if (id === null) {
      return MapConverter.convert(null);
    }

    return MapConverter.convert(LocationRepository.getLocationById(id));
  }

  public static isInBattle(locationId: string | null): boolean {
    return locationId === BATTLE_LOCATION_ID;
  }

  public static isInCardBattle(locationId: string | null): boolean {
    return locationId === CARD_BATTLE_LOCATION_ID;
  }
}
