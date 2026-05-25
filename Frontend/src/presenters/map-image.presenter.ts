import { LocationRepository } from "@/repositories/location-repository";
import type { LocationViewModel } from "@/view-models/location-view-model";

export class MapImagePresenter {
    public static getLocationById(locationId: string): LocationViewModel {
        return LocationRepository.getLocationById(locationId);
    }
}