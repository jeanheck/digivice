import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";
import type { ZoomedLocationMapViewModel } from "@/viewmodels/quest/zoomed-location-map.viewmodel";

export class ZoomedLocationMapConverter {
    public static convert(
        imageUrl: string | null,
        coordinates: CoordinatesViewModel,
        labelKey: string
    ): ZoomedLocationMapViewModel {
        return {
            imageUrl,
            coordinates,
            labelKey,
        };
    }
}
