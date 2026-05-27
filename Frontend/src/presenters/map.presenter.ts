import { LocationRepository } from "@/repositories/location.repository";
import type { LocationViewModel } from "@/viewmodels/location.viewmodel";

export class MapPresenter {
    public static getLocationById(locationId: string): LocationViewModel {
        return LocationRepository.getLocationById(locationId);
    }
}