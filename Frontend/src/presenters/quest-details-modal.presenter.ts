import { ImageCatalog } from "@/catalogs/image.catalog";
import { LocationRepository } from "@/repositories/location.repository";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

export class QuestDetailsModalPresenter {
    public static getLocationById(locationId: string): LocationViewModel {
        return LocationRepository.getLocationById(locationId);
    }

    public static getLocalMapUrl(locationId: string | undefined): string | null {
        if (!locationId) {
            return null;
        }

        const locationViewModel = this.getLocationById(locationId);
        return ImageCatalog.getMapImageUrl(locationViewModel.image);
    }
}