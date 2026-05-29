import type { CoordinatesViewModel } from "./coordinates.viewmodel";

export interface ZoomedLocationMapViewModel {
    imageUrl: string | null;
    coordinates: CoordinatesViewModel;
    labelKey: string;
}
