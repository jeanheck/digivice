import { MapConverter } from "@/presenters/converter/map.converter";
import { LocationRepository } from "@/repositories/location.repository";
import type { MapViewModel } from "@/viewmodels/map/map.viewmodel";

export class MapPresenter {
  public static getByLocationId(id: string | null): MapViewModel {
    if (id === null) {
      return MapConverter.convert(null);
    }

    return MapConverter.convert(LocationRepository.getLocationById(id));
  }
}
