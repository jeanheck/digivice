import type { CoordinatesViewModel } from "./coordinates-view-model";

export interface ZoomedLocationViewModel
{
    locationImage: string,
    coordinates: CoordinatesViewModel;
}