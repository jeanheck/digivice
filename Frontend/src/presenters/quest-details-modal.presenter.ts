import { LocationRepository } from "@/repositories/location.repository";
import type { LocationViewModel } from "@/viewmodels/location.viewmodel";

export class QuestDetailsModalPresenter {
    public static getLocationById(locationId: string): LocationViewModel {
        return LocationRepository.getLocationById(locationId);
    }
}