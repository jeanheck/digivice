import asukaMapUrl from "@/assets/AsukaMap.webp";
import { ImageCatalog } from "@/catalogs/image.catalog";
import { LocationRepository } from "@/repositories/location.repository";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";
import type { ZoomedLocationMapViewModel } from "@/viewmodels/quest/zoomed-location-map.viewmodel";

export class QuestModalPresenter {
    public static getWorldMapLocations(selectedStep: StepViewModel | null): ZoomedLocationMapViewModel[] {
        if (!selectedStep?.location || !selectedStep.coordinates) {
            return [];
        }

        return [{
            imageUrl: asukaMapUrl,
            coordinates: selectedStep.coordinates,
            labelKey: `location.${selectedStep.location}`,
        }];
    }

    public static getLocalMapLocations(
        selectedStep: StepViewModel | null,
        questId: string | null
    ): ZoomedLocationMapViewModel[] {
        if (!selectedStep?.zoomedLocations?.length || !questId) {
            return [];
        }

        return selectedStep.zoomedLocations.map((zoomedLocation, locationIndex) => {
            return {
                imageUrl: QuestModalPresenter.getLocalMapUrl(zoomedLocation.location),
                coordinates: zoomedLocation.coordinates,
                labelKey: `${questId}.steps.${selectedStep.number}.locations.${locationIndex}.locationTarget`,
            };
        });
    }

    private static getLocalMapUrl(locationId: string | undefined): string | null {
        if (!locationId) {
            return null;
        }

        const locationViewModel = LocationRepository.getLocationById(locationId);
        return ImageCatalog.getMapImageUrl(locationViewModel.image);
    }
}
